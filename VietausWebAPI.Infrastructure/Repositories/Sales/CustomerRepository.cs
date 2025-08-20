using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewCustomer(Customer1 customer)
        {
            await _context.Customers1.AddAsync(customer);
        }

        public async Task<bool> DeleteCustomerByIdAsync(Guid id)
        {
            var customer = await _context.Customers1.FindAsync(id);
            if (customer == null)
            {
                return false; // Không tìm thấy khách hàng
            }
            customer.IsActive = false; // Đánh dấu là không hoạt động
            _context.Customers1.Update(customer);
            return true;
        }

        public async Task<PagedResult<Customer1>> GetAllAsync(CustomerQuery? query)
        {
            var queryAble = _context.Customers1
                .Where(x => x.IsActive == true) // Chỉ lấy khách hàng đang hoạt động
                .Include(c => c.CustomerAssignments)
                    .ThenInclude(ca => ca.Employee)
                //.Include(x => x.UpdatedBy)
                .Include(c => c.Addresses)
                .Include(c => c.Contacts)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                string keywordLower = query.keyword.ToLower();
                queryAble = queryAble.Where(x =>
                    x.CustomerName != null && x.ApplicationName != null &&
                    EF.Functions.Collate(x.ApplicationName, "Latin1_General_CI_AI").ToLower().Contains(keywordLower)
                    ||
                    x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keywordLower)
                );
            }

            queryAble = queryAble.OrderByDescending(x => x.UpdatedDate);

            return await QueryableExtensions.GetPagedAsync(queryAble, query);

        }

        public async Task<GetCustomer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers1
                   .AsNoTracking()
                   .Where(c => c.CustomerId == id)
                   .Select(c => new GetCustomer
                   {
                       CustomerId = c.CustomerId,
                       ExternalId = c.ExternalId,
                       CustomerName = c.CustomerName,
                       //EmployeeName = c.Employee != null ? c.Employee.FullName : null,
                       CustomerGroup = c.CustomerGroup,
                       ApplicationName = c.ApplicationName,
                       RegistrationNumber = c.RegistrationNumber,
                       TaxNumber = c.TaxNumber,
                       Phone = c.Phone,
                       Website = c.Website,
                       UpdatedDate = c.UpdatedDate,
                       UpdatedBy = c.UpdatedBy,
                       IssueDate = c.IssueDate,
                       IssuedPlace = c.IssuedPlace,
                       FaxNumber = c.FaxNumber,
                       Product = c.Product,
                       //IsActive = c.IsActive,
                       // CreatedDate = c.CreatedDate,
                       // CreatedBy = c.CreatedBy,
                       CompanyName = c.Company != null ? c.Company.Name : null,
                        

                       // Giới hạn collection để nhẹ dữ liệu
                       Addresses = c.Addresses
                           .OrderByDescending(a => a.IsPrimary == true)
                           .Select(a => new GetAddress
                           {
                               District = a.District,
                               Province = a.Province,
                               Country = a.Country,
                               AddressId = a.AddressId,
                               AddressLine = a.AddressLine,
                               City = a.City,
                               IsPrimary = a.IsPrimary,
                               PostalCode = a.PostalCode
                           })
                           .ToList(),

                       Contacts = c.Contacts
                           .OrderByDescending(k => k.IsPrimary == true)
                           .Take(5)
                           .Select(k => new GetContact
                           {
                               IsPrimary = k.IsPrimary,
                               ContactId = k.ContactId,
                               FirstName = k.FirstName,
                               Gender = k.Gender,
                               LastName = k.LastName,
                               Email = k.Email,
                               Phone = k.Phone
                           })
                           .ToList()
                   })
                   .FirstOrDefaultAsync();
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.Customers1
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId.Length)   // dài hơn => số lớn hơn
                .ThenByDescending(e => e.ExternalId)           // cùng độ dài thì so chuỗi
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Customer1> Query()
        {
            return _context.Customers1.AsNoTracking();
        }

        public async Task<bool> UpdateCustomerAsync(PatchCustomer customer)
        {
            var existingCustomer = await _context.Customers1
                .Include(c => c.Addresses)
                .Include(c => c.Contacts)
                .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

            if (existingCustomer == null)
            {
                return false; // Không tìm thấy khách hàng
            }

            // --- STEP 1: Cập nhật thông tin cơ bản ---
            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Website = customer.Website;
            existingCustomer.UpdatedDate = DateTime.UtcNow;
            existingCustomer.UpdatedBy = customer.UpdatedBy;
            existingCustomer.CustomerGroup = customer.CustomerGroup;
            existingCustomer.ApplicationName = customer.ApplicationName;
            existingCustomer.RegistrationNumber = customer.RegistrationNumber;
            existingCustomer.IssueDate = customer.IssueDate;
            existingCustomer.IssuedPlace = customer.IssuedPlace;
            existingCustomer.FaxNumber = customer.FaxNumber;
            existingCustomer.Product = customer.Product;
            existingCustomer.TaxNumber = customer.TaxNumber;
            existingCustomer.CompanyId = customer.CompanyId;
            existingCustomer.IsActive = customer.IsActive;

            // --- STEP 2: Cập nhật địa chỉ ---
            var incomingAddresses = customer.Addresses;
            var existingAddresses = existingCustomer.Addresses.ToList();

            // XOÁ các địa chỉ không còn trong incomingAddresses

            //foreach (var existingAddress in existingAddresses)
            //{
            //    if (!incomingAddresses.Any(a => a.AddressId == existingAddress.AddressId))
            //    {
            //        _context.Addresses.Remove(existingAddress);
            //    }
            //}

            var incomingAddressIds = incomingAddresses.Select(a => a.AddressId).ToHashSet();

            foreach (var existingAddress in existingAddresses)
            {
                if (!incomingAddressIds.Contains(existingAddress.AddressId))
                {
                    _context.Addresses.Remove(existingAddress);
                }
            }


            // CẬP NHẬT hoặc THÊM mới địa chỉ
            foreach (var incomingAddress in incomingAddresses)
            {
                // Nếu là Address mới chưa có ID → thêm luôn
                if (incomingAddress.AddressId == Guid.Empty)
                {
                    var newAddress = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        CustomerId = existingCustomer.CustomerId,
                        AddressLine = incomingAddress.AddressLine,
                        City = incomingAddress.City,
                        District = incomingAddress.District,
                        Province = incomingAddress.Province,
                        Country = incomingAddress.Country,
                        PostalCode = incomingAddress.PostalCode,
                        IsPrimary = incomingAddress.IsPrimary
                    };
                    _context.Addresses.Add(newAddress);
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
                        var newAddress = new Address
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
                        _context.Addresses.Add(newAddress);
                    }
                }
            }


            // --- STEP 3: Cập nhật liên hệ ---
            var incomingContacts = customer.Contacts;
            var existingContacts = existingCustomer.Contacts.ToList();

            // XOÁ các địa chỉ không còn trong incomingAddresses

            //foreach (var existingContact in existingContacts)
            //{
            //    if (!incomingContacts.Any(a => a.ContactId == existingContact.ContactId))
            //    {
            //        _context.Contacts.Remove(existingContact);
            //    }
            //}

            var incomingContectIds = incomingContacts.Select(a => a.ContactId).ToHashSet();

            foreach (var existingContact in existingContacts)
            {
                if (!incomingContectIds.Contains(existingContact.ContactId))
                {
                    _context.Contacts.Remove(existingContact);
                }
            }


            // CẬP NHẬT hoặc THÊM mới địa chỉ
            foreach (var incomingContact in incomingContacts)
            {
                if (incomingContact.ContactId == Guid.Empty)
                {
                    var newContact = new Contact
                    {
                        ContactId = Guid.NewGuid(),
                        CustomerId = existingCustomer.CustomerId,
                        FirstName = incomingContact.FirstName,
                        LastName = incomingContact.LastName,
                        Email = incomingContact.Email,
                        Phone = incomingContact.Phone,
                        IsPrimary = incomingContact.IsPrimary,
                        Gender = incomingContact.Gender
                    };
                    _context.Contacts.Add(newContact);
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
                        var newContact = new Contact
                        {
                            ContactId = incomingContact.ContactId,
                            FirstName = incomingContact.FirstName,
                            LastName = incomingContact.LastName,
                            Email = incomingContact.Email,
                            Phone = incomingContact.Phone,
                            IsPrimary = incomingContact.IsPrimary,
                            Gender = incomingContact.Gender
                        };
                        _context.Contacts.Add(newContact);
                    }
                }
            }


            //await _context.SaveChangesAsync();

            return true;
        }
    }
}
