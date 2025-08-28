using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewSuplier(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
        }

        public IQueryable<Supplier> Query()
        {
            return _context.Suppliers.AsQueryable();
        }

        public async Task<bool> DeleteSupplierByIdAsync(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return false; // Không tìm thấy khách hàng
            }
            supplier.IsActive = false; // Đánh dấu là không hoạt động
            _context.Suppliers.Update(supplier);
            return true;
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.Suppliers
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId.Length)   // dài hơn => số lớn hơn
                .ThenByDescending(e => e.ExternalId)           // cùng độ dài thì so chuỗi
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateSupplierAsync(PatchSupplier supplier)
        {
            var existingSupplier = await _context.Suppliers
                .Include(c => c.SupplierAddresses)
                .Include(c => c.SupplierContacts)
                .FirstOrDefaultAsync(c => c.SupplierId == supplier.SupplierId);

            if (existingSupplier == null)
            {
                return false; // Không tìm thấy khách hàng
            }

            // --- STEP 1: Cập nhật thông tin cơ bản ---
            existingSupplier.SupplierName = supplier.SupplierName;
            existingSupplier.Phone = supplier.Phone;
            existingSupplier.Website = supplier.Website;
            existingSupplier.UpdatedDate = DateTime.Now;
            existingSupplier.UpdatedBy = supplier.UpdatedBy;
            existingSupplier.RegistrationNumber = supplier.RegistrationNumber;
            existingSupplier.IssueDate = supplier.IssueDate;
            existingSupplier.IssuedPlace = supplier.IssuedPlace;
            existingSupplier.FaxNumber = supplier.FaxNumber;
            existingSupplier.TaxNumber = supplier.TaxNumber;
            existingSupplier.IsActive = supplier.IsActive;

            // --- STEP 2: Cập nhật địa chỉ ---
            var incomingAddresses = supplier.SupplierAddresses;
            var existingAddresses = existingSupplier.SupplierAddresses.ToList();

            // XOÁ các địa chỉ không còn trong incomingAddresse
            var incomingAddressIds = incomingAddresses.Select(a => a.AddressId).ToHashSet();

            foreach (var existingAddress in existingAddresses)
            {
                if (!incomingAddressIds.Contains(existingAddress.AddressId))
                {
                    _context.SupplierAddresses.Remove(existingAddress);
                }
            }


            // CẬP NHẬT hoặc THÊM mới địa chỉ
            foreach (var incomingAddress in incomingAddresses)
            {
                // Nếu là Address mới chưa có ID → thêm luôn
                if (incomingAddress.AddressId == Guid.Empty)
                {
                    var newAddress = new SupplierAddress
                    {
                        AddressId = Guid.NewGuid(),
                        SupplierId = existingSupplier.SupplierId,
                        AddressLine = incomingAddress.AddressLine,
                        City = incomingAddress.City,
                        District = incomingAddress.District,
                        Province = incomingAddress.Province,
                        Country = incomingAddress.Country,
                        PostalCode = incomingAddress.PostalCode,
                        IsPrimary = incomingAddress.IsPrimary
                    };
                    _context.SupplierAddresses.Add(newAddress);
                }
                else
                {
                    var existingAddress = existingAddresses
                        .FirstOrDefault(a => a.AddressId == incomingAddress.AddressId);

                    if (existingAddress != null)
                    {
                        // Cập nhật
                        existingAddress.AddressLine = incomingAddress.AddressLine;
                        existingAddress.City = incomingAddress.City;
                        existingAddress.District = incomingAddress.District;
                        existingAddress.Province = incomingAddress.Province;
                        existingAddress.Country = incomingAddress.Country;
                        existingAddress.PostalCode = incomingAddress.PostalCode;
                        existingAddress.IsPrimary = incomingAddress.IsPrimary;
                    }
                    else
                    {
                        // Dù có ID nhưng không thấy trong DB → thêm mới
                        var newAddress = new SupplierAddress
                        {
                            AddressId = incomingAddress.AddressId,
                            AddressLine = incomingAddress.AddressLine,
                            City = incomingAddress.City,
                            District = incomingAddress.District,
                            Province = incomingAddress.Province,
                            Country = incomingAddress.Country,
                            PostalCode = incomingAddress.PostalCode,
                            IsPrimary = incomingAddress.IsPrimary
                        };
                        _context.SupplierAddresses.Add(newAddress);
                    }
                }
            }


            // --- STEP 3: Cập nhật liên hệ ---
            var incomingContacts = supplier.SupplierContacts;
            var existingContacts = existingSupplier.SupplierContacts.ToList();

            // XOÁ các địa chỉ không còn trong incomingAddresses
            var incomingContectIds = incomingContacts.Select(a => a.ContactId).ToHashSet();

            foreach (var existingContact in existingContacts)
            {
                if (!incomingContectIds.Contains(existingContact.ContactId))
                {
                    _context.SupplierContacts.Remove(existingContact);
                }
            }


            // CẬP NHẬT hoặc THÊM mới địa chỉ
            foreach (var incomingContact in incomingContacts)
            {
                if (incomingContact.ContactId == Guid.Empty)
                {
                    var newContact = new SupplierContact
                    {
                        ContactId = Guid.NewGuid(),
                        SupplierId = existingSupplier.SupplierId,
                        FirstName = incomingContact.FirstName,
                        LastName = incomingContact.LastName,
                        Email = incomingContact.Email,
                        Phone = incomingContact.Phone,
                        IsPrimary = incomingContact.IsPrimary,
                        Gender = incomingContact.Gender
                    };
                    _context.SupplierContacts.Add(newContact);
                }
                else
                {
                    var existingContact = existingContacts
                        .FirstOrDefault(c => c.ContactId == incomingContact.ContactId);

                    if (existingContact != null)
                    {
                        existingContact.FirstName = incomingContact.FirstName;
                        existingContact.LastName = incomingContact.LastName;
                        existingContact.Email = incomingContact.Email;
                        existingContact.Phone = incomingContact.Phone;
                        existingContact.IsPrimary = incomingContact.IsPrimary;
                        existingContact.Gender = incomingContact.Gender;
                    }
                    else
                    {
                        var newContact = new SupplierContact
                        {
                            ContactId = incomingContact.ContactId,
                            FirstName = incomingContact.FirstName,
                            LastName = incomingContact.LastName,
                            Email = incomingContact.Email,
                            Phone = incomingContact.Phone,
                            IsPrimary = incomingContact.IsPrimary,
                            Gender = incomingContact.Gender
                        };
                        _context.SupplierContacts.Add(newContact);
                    }
                }
            }


            //await _context.SaveChangesAsync();

            return true;
        }
    }
}
