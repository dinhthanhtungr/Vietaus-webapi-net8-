using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.PatchDtos;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Helpers.SupplierFeatures
{
    public static class SupplierAggregateSync 
    {

        public static List<SupplierContact> SyncContacts(
            Supplier supplier,
            List<PatchSupplierContact>? incoming,
            bool treatMissingAsSoftDelete = false)
        {
            incoming ??= new();
            var added = new List<SupplierContact>();

            var incomingById = incoming
                .Where(x => x.ContactId != Guid.Empty)
                .ToDictionary(x => x.ContactId, x => x);

            // 0) stale check: incoming có id != empty nhưng supplier hiện không có
            var existingIds = supplier.SupplierContacts.Select(x => x.ContactId).ToHashSet();
            var staleIds = incomingById.Keys.Where(id => !existingIds.Contains(id)).ToList();
            if (staleIds.Count > 0)
                throw new InvalidOperationException(
                    $"Có ContactId không còn tồn tại trong DB (stale). Vui lòng reload. First={staleIds[0]}");

            // (Optional) Soft delete những cái không còn trong incoming (chỉ dùng khi FE gửi FULL LIST)
            if (treatMissingAsSoftDelete)
            {
                foreach (var c in supplier.SupplierContacts.Where(x => x.IsActive == true))
                    if (!incomingById.ContainsKey(c.ContactId))
                    {
                        c.IsActive = false;
                        c.IsPrimary = false;
                    }
            }

            // 1) Update existing
            foreach (var existing in supplier.SupplierContacts)
            {
                if (!incomingById.TryGetValue(existing.ContactId, out var dto))
                    continue;

                PatchHelper.SetIfRef(dto.FirstName, () => existing.FirstName, v => existing.FirstName = v);
                PatchHelper.SetIfRef(dto.LastName, () => existing.LastName, v => existing.LastName = v);
                PatchHelper.SetIfRef(dto.Email, () => existing.Email, v => existing.Email = v);
                PatchHelper.SetIfRef(dto.Phone, () => existing.Phone, v => existing.Phone = v);

                PatchHelper.SetIfRef(dto.Gender, () => existing.Gender, v => existing.Gender = v);
                var newIsActive = dto.IsActive; // bool? => null nghĩa là "không patch"
                if (newIsActive.HasValue)
                {
                    PatchHelper.SetIfNullable(dto.IsActive, () => existing.IsActive, v => existing.IsActive = v);

                    if (newIsActive.Value == false)
                    {
                        existing.IsPrimary = false;
                    }
                    else
                    {
                        // chỉ patch primary nếu active
                        PatchHelper.SetIfNullable(dto.IsPrimary, () => existing.IsPrimary, v => existing.IsPrimary = v);
                    }
                }
                else
                {
                    // Không patch IsActive => vẫn có thể patch IsPrimary nếu bạn muốn
                    PatchHelper.SetIfNullable(dto.IsPrimary, () => existing.IsPrimary, v => existing.IsPrimary = v);
                }
            }

            // 2) Add new
            foreach (var dto in incoming.Where(x => x.ContactId == Guid.Empty))
            {
                var c = new SupplierContact
                {
                    ContactId = Guid.CreateVersion7(),
                    SupplierId = supplier.SupplierId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Gender = dto.Gender,
                    IsPrimary = dto.IsPrimary,
                    IsActive = dto.IsActive ?? true
                };

                supplier.SupplierContacts.Add(c); // giữ aggregate
                added.Add(c);                     // trả ra cho repo.AddRangeAsync (nếu cần)
            }

            // 3) Normalize primary (chỉ trên IsActive=true)
            NormalizePrimaryContact(supplier);

            return added;
        }
        public static List<SupplierAddress> SyncAddresses(
            Supplier supplier,
            List<PatchSupplierAddress>? incoming,
            bool treatMissingAsSoftDelete = false)
        {
            incoming ??= new();
            var added = new List<SupplierAddress>();

            var incomingById = incoming
                .Where(x => x.AddressId != Guid.Empty)
                .ToDictionary(x => x.AddressId, x => x);

            // stale check (incoming có id nhưng supplier hiện không có)
            var existingIds = supplier.SupplierAddresses.Select(x => x.AddressId).ToHashSet();
            var staleIds = incomingById.Keys.Where(id => !existingIds.Contains(id)).ToList();
            if (staleIds.Count > 0)
                throw new InvalidOperationException(
                    $"Có AddressId không còn tồn tại trong DB (stale). Vui lòng reload. First={staleIds[0]}");

            // soft delete nếu FE gửi full list
            if (treatMissingAsSoftDelete)
            {
                foreach (var a in supplier.SupplierAddresses.Where(x => x.IsActive == true))
                    if (!incomingById.ContainsKey(a.AddressId))
                    {
                        a.IsActive = false;
                        a.IsPrimary = false;
                    }
            }

            // update existing
            foreach (var existing in supplier.SupplierAddresses)
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
                if (dto.IsActive.HasValue)
                {
                    PatchHelper.SetIfNullable(dto.IsActive, () => existing.IsActive, v => existing.IsActive = v);
                    if (dto.IsActive.Value == false)
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
                var a = new SupplierAddress
                {
                    AddressId = Guid.CreateVersion7(),
                    SupplierId = supplier.SupplierId,
                    AddressLine = dto.AddressLine,
                    City = dto.City,
                    District = dto.District,
                    Province = dto.Province,
                    Country = dto.Country,
                    PostalCode = dto.PostalCode,
                    IsPrimary = dto.IsPrimary,
                    IsActive = dto.IsActive ?? true
                };

                supplier.SupplierAddresses.Add(a);
                added.Add(a);
            }

            NormalizePrimaryAddress(supplier);

            return added;
        }



        private static void NormalizePrimaryAddress(Supplier supplier)
        {
            var actives = supplier.SupplierAddresses.Where(x => x.IsActive == true).ToList();
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

        private static void NormalizePrimaryContact(Supplier supplier)
        {
            var actives = supplier.SupplierContacts.Where(x => x.IsActive == true).ToList();
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
