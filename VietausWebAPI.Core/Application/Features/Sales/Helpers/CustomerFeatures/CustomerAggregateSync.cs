using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerFeatures
{
    public static class CustomerAggregateSync
    {
        public static List<Contact> SyncContacts(
            Customer customer,
            List<PatchContact>? incoming,
            bool treatMissingAsSoftDelete = false)
        {
            incoming ??= new();

            var added = new List<Contact>();
            var incomingById = incoming
                .Where(x => x.ContactId != Guid.Empty)
                .ToDictionary(x => x.ContactId, x => x);

            // stale check
            var existingIds = customer.Contacts.Select(x => x.ContactId).ToHashSet();
            var staleIds = incomingById.Keys.Where(id => !existingIds.Contains(id)).ToList();
            if (staleIds.Count > 0)
                throw new InvalidOperationException(
                    $"Có ContactId không còn tồn tại trong DB (stale). Vui lòng reload. First={staleIds[0]}");

            if (treatMissingAsSoftDelete)
            {
                foreach (var c in customer.Contacts.Where(x => x.IsActive == true))
                    if (!incomingById.ContainsKey(c.ContactId))
                    {
                        c.IsActive = false;
                        c.IsPrimary = false;
                    }
            }

            // Update existing
            foreach (var existing in customer.Contacts)
            {
                if (!incomingById.TryGetValue(existing.ContactId, out var dto))
                    continue;

                PatchHelper.SetIfRef(dto.FirstName, () => existing.FirstName, v => existing.FirstName = v);
                PatchHelper.SetIfRef(dto.LastName, () => existing.LastName, v => existing.LastName = v);
                PatchHelper.SetIfRef(dto.Email, () => existing.Email, v => existing.Email = v);
                PatchHelper.SetIfRef(dto.Phone, () => existing.Phone, v => existing.Phone = v);
                PatchHelper.SetIfRef(dto.Gender, () => existing.Gender, v => existing.Gender = v);

                var newIsActive = dto.IsActive; // bool?
                if (newIsActive != null)
                {
                    PatchHelper.SetIf(dto.IsActive, () => existing.IsActive, v => existing.IsActive = v);

                    if (newIsActive == false)
                    {
                        existing.IsPrimary = false;
                    }
                    else
                    {
                        PatchHelper.SetIfNullable(dto.IsPrimary, () => existing.IsPrimary, v => existing.IsPrimary = v);
                    }
                }
                else
                {
                    PatchHelper.SetIfNullable(dto.IsPrimary, () => existing.IsPrimary, v => existing.IsPrimary = v);
                }
            }

            // Add new
            foreach (var dto in incoming.Where(x => x.ContactId == Guid.Empty))
            {
                var c = new Contact
                {
                    ContactId = Guid.CreateVersion7(),
                    CustomerId = customer.CustomerId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Gender = dto.Gender,
                    IsPrimary = dto.IsPrimary,
                    IsActive = dto.IsActive
                };

                customer.Contacts.Add(c);
                added.Add(c);
            }

            NormalizePrimaryContact(customer);
            return added;
        }




        public static List<Address> SyncAddresses(
            Customer customer,
            List<PatchAddress>? incoming,
            bool treatMissingAsSoftDelete = false)
        {
            incoming ??= new();
            var added = new List<Address>();

            var incomingById = incoming
                .Where(x => x.AddressId != Guid.Empty)
                .ToDictionary(x => x.AddressId, x => x);

            // stale check (incoming có id nhưng supplier hiện không có)
            var existingIds = customer.Addresses.Select(x => x.AddressId).ToHashSet();
            var staleIds = incomingById.Keys.Where(id => !existingIds.Contains(id)).ToList();
            if (staleIds.Count > 0)
                throw new InvalidOperationException(
                    $"Có AddressId không còn tồn tại trong DB (stale). Vui lòng reload. First={staleIds[0]}");

            // soft delete nếu FE gửi full list
            if (treatMissingAsSoftDelete)
            {
                foreach (var a in customer.Addresses.Where(x => x.IsActive == true))
                    if (!incomingById.ContainsKey(a.AddressId))
                    {
                        a.IsActive = false;
                        a.IsPrimary = false;
                    }
            }

            // update existing
            foreach (var existing in customer.Addresses)
            {
                if (!incomingById.TryGetValue(existing.AddressId, out var dto))
                    continue;

                PatchHelper.SetIfRef(dto.AddressLine, () => existing.AddressLine, v => existing.AddressLine = v);
                PatchHelper.SetIfRef(dto.City, () => existing.City, v => existing.City = v);
                PatchHelper.SetIfRef(dto.District, () => existing.District, v => existing.District = v);
                PatchHelper.SetIfRef(dto.Province, () => existing.Province, v => existing.Province = v);
                PatchHelper.SetIfRef(dto.Country, () => existing.Country, v => existing.Country = v);
                PatchHelper.SetIfRef(dto.PostalCode, () => existing.PostalCode, v => existing.PostalCode = v);

                // IsActive/IsPrimary
                if (dto.IsActive != null)
                {
                    PatchHelper.SetIf(dto.IsActive, () => existing.IsActive, v => existing.IsActive = v);
                    if (dto.IsActive == false)
                        existing.IsPrimary = false;
                    else
                        PatchHelper.SetIfNullable(dto.IsPrimary, () => existing.IsPrimary, v => existing.IsPrimary = v);
                }
                else
                {
                    PatchHelper.SetIfNullable(dto.IsPrimary, () => existing.IsPrimary, v => existing.IsPrimary = v);
                }
            }

            // add new
            foreach (var dto in incoming.Where(x => x.AddressId == Guid.Empty))
            {
                var a = new Address
                {
                    AddressId = Guid.CreateVersion7(),
                    CustomerId = customer.CustomerId,
                    AddressLine = dto.AddressLine,
                    City = dto.City,
                    District = dto.District,
                    Province = dto.Province,
                    Country = dto.Country,
                    PostalCode = dto.PostalCode,
                    IsPrimary = dto.IsPrimary,
                    IsActive = dto.IsActive
                };

                customer.Addresses.Add(a);
                added.Add(a);
            }

            NormalizePrimaryAddress(customer);
            return added;
        }



        private static void NormalizePrimaryAddress(Customer customer)
        {
            var actives = customer.Addresses.Where(x => x.IsActive == true).ToList();
            if (actives.Count == 0) return;

            // nếu có nhiều primary, giữ cái đầu tiên (hoặc bạn đổi rule theo ý)
            var firstPrimary = actives.FirstOrDefault(x => x.IsPrimary == true);
            if (firstPrimary == null)
            {
                // không có primary -> set cái đầu tiên làm primary cho ổn định UI
                actives[0].IsPrimary = true;
                return;
            }

            foreach (var a in actives)
                if (a.AddressId != firstPrimary.AddressId)
                    a.IsPrimary = false;
        }

        private static void NormalizePrimaryContact(Customer customer)
        {
            var actives = customer.Contacts.Where(x => x.IsActive == true).ToList();
            if (actives.Count == 0) return;

            var firstPrimary = actives.FirstOrDefault(x => x.IsPrimary == true);
            if (firstPrimary == null)
            {
                actives[0].IsPrimary = true;
                return;
            }

            foreach (var c in actives)
                if (c.ContactId != firstPrimary.ContactId)
                    c.IsPrimary = false;
        }
    }

}
