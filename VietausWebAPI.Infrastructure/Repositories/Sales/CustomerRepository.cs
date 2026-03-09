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
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Utilities;



namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> Query(bool track = false)
        {
            var db = _context.Customers.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        /// <summary>
        /// Thêm một khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task AddNewCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        /// <summary>
        /// Xóa mềm dữ liệu khách hàng này
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCustomerByIdAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false; // Không tìm thấy khách hàng
            }
            customer.IsActive = false; // Đánh dấu là không hoạt động
            _context.Customers.Update(customer);
            return true;
        }

        public async Task<PagedResult<Customer>> GetAllAsync(CustomerQuery? query)
        {
            var queryAble = _context.Customers
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
                    (x.ApplicationName != null && x.ApplicationName.ToLower().Contains(keywordLower)) ||
                    (x.ExternalId != null && x.ExternalId.ToLower().Contains(keywordLower))
                );
            }


            queryAble = queryAble.OrderByDescending(x => x.UpdatedDate);

            return await QueryableExtensions.GetPagedAsync(queryAble, query);

        }

        public async Task<GetCustomer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == id)
                .Select(c => new GetCustomer
                {
                    CustomerId = c.CustomerId,
                    ExternalId = c.ExternalId,
                    CustomerName = c.CustomerName,
                    CustomerGroup = c.CustomerGroup,
                    ApplicationName = c.ApplicationName,
                    RegistrationNumber = c.RegistrationNumber,
                    RegistrationAddress = c.RegistrationAddress,
                    TaxNumber = c.TaxNumber,
                    Phone = c.Phone,
                    Website = c.Website,
                    IssueDate = c.IssueDate,
                    IssuedPlace = c.IssuedPlace,
                    FaxNumber = c.FaxNumber,

                    CompanyName = c.Company != null ? c.Company.Name : null,

                    Addresses = c.Addresses
                        .Where(a => a.IsActive == true)
                        .OrderByDescending(a => a.IsPrimary)
                        .Select(a => new GetAddress
                        {
                            AddressId = a.AddressId,
                            AddressLine = a.AddressLine,
                            City = a.City,
                            District = a.District,
                            Province = a.Province,
                            Country = a.Country,
                            PostalCode = a.PostalCode,
                            IsPrimary = a.IsPrimary,
                            IsActive = a.IsActive,
                        })
                        .ToList(),

                    Contacts = c.Contacts
                        .Where(k => k.IsActive == true)
                        .OrderByDescending(k => k.IsPrimary)
                        .Take(5)
                        .Select(k => new GetContact
                        {
                            ContactId = k.ContactId,
                            FirstName = k.FirstName,
                            LastName = k.LastName,
                            Gender = k.Gender,
                            Email = k.Email,
                            Phone = k.Phone,
                            IsPrimary = k.IsPrimary,
                            IsActive = k.IsActive,
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }


        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            var sql = """
                    SELECT c."ExternalId"
                    FROM "Customer"."Customer" c
                    WHERE c."ExternalId" ~ ('^' || @p0 || '_[0-9]+$')
                    ORDER BY (SUBSTRING(c."ExternalId" FROM '([0-9]+)$'))::bigint DESC
                    LIMIT 1;
                    """;

            return await _context.Database.SqlQueryRaw<string>(sql, prefix).FirstOrDefaultAsync();
        }

        public IQueryable<Address> QueryAddress(bool track = false)
        {
            var db = _context.Addresses.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public IQueryable<Contact> QueryContact(bool track = false)
        {
            var db = _context.Contacts.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        //public async Task<bool> UpdateCustomerAsync(PatchCustomer customer)
        //{
        //    var existingCustomer = await _context.Customers
        //        .Include(c => c.Addresses)
        //        .Include(c => c.Contacts)
        //        .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        //    if (existingCustomer == null)
        //    {
        //        return false; // Không tìm thấy khách hàng
        //    }

        //    // --- STEP 1: Cập nhật thông tin cơ bản ---
        //    existingCustomer.CustomerName = customer.CustomerName;
        //    existingCustomer.Phone = customer.Phone;
        //    existingCustomer.Website = customer.Website;
        //    existingCustomer.UpdatedDate = DateTime.Now;
        //    existingCustomer.UpdatedBy = customer.UpdatedBy;
        //    existingCustomer.CustomerGroup = customer.CustomerGroup;
        //    existingCustomer.ApplicationName = customer.ApplicationName;
        //    existingCustomer.RegistrationNumber = customer.RegistrationNumber;
        //    existingCustomer.IssueDate = customer.IssueDate;
        //    existingCustomer.IssuedPlace = customer.IssuedPlace;
        //    existingCustomer.FaxNumber = customer.FaxNumber;
        //    existingCustomer.TaxNumber = customer.TaxNumber;
        //    //existingCustomer.CompanyId = customer.CompanyId;
        //    existingCustomer.IsActive = customer.IsActive;

        //    // --- STEP 2: Cập nhật địa chỉ ---
        //    var incomingAddresses = customer.Addresses;
        //    var existingAddresses = existingCustomer.Addresses.ToList();

        //    // XOÁ các địa chỉ không còn trong incomingAddresse
        //    var incomingAddressIds = incomingAddresses.Select(a => a.AddressId).ToHashSet();

        //    foreach (var existingAddress in existingAddresses)
        //    {
        //        if (!incomingAddressIds.Contains(existingAddress.AddressId))
        //        {
        //            _context.Addresses.Remove(existingAddress);
        //        }
        //    }


        //    // CẬP NHẬT hoặc THÊM mới địa chỉ
        //    foreach (var incomingAddress in incomingAddresses)
        //    {
        //        // Nếu là Address mới chưa có ID → thêm luôn
        //        if (incomingAddress.AddressId == Guid.Empty)
        //        {
        //            var newAddress = new Address
        //            {
        //                AddressId = Guid.CreateVersion7(),
        //                CustomerId = existingCustomer.CustomerId,
        //                AddressLine = incomingAddress.AddressLine,
        //                City = incomingAddress.City,
        //                District = incomingAddress.District,
        //                Province = incomingAddress.Province,
        //                Country = incomingAddress.Country,
        //                PostalCode = incomingAddress.PostalCode,
        //                IsPrimary = incomingAddress.IsPrimary
        //            };
        //            _context.Addresses.Add(newAddress);
        //        }
        //        else
        //        {
        //            var existingAddress = existingAddresses
        //                .FirstOrDefault(a => a.AddressId == incomingAddress.AddressId);

        //            if (existingAddress != null)
        //            {
        //                // Cập nhật
        //                existingAddress.AddressLine = incomingAddress.AddressLine;
        //                existingAddress.City = incomingAddress.City;
        //                existingAddress.District = incomingAddress.District;
        //                existingAddress.Province = incomingAddress.Province;
        //                existingAddress.Country = incomingAddress.Country;
        //                existingAddress.PostalCode = incomingAddress.PostalCode;
        //                existingAddress.IsPrimary = incomingAddress.IsPrimary;
        //            }
        //            else
        //            {
        //                // Dù có ID nhưng không thấy trong DB → thêm mới
        //                var newAddress = new Address
        //                {
        //                    AddressId = incomingAddress.AddressId,
        //                    AddressLine = incomingAddress.AddressLine,
        //                    City = incomingAddress.City,
        //                    District = incomingAddress.District,
        //                    Province = incomingAddress.Province,
        //                    Country = incomingAddress.Country,
        //                    PostalCode = incomingAddress.PostalCode,
        //                    IsPrimary = incomingAddress.IsPrimary
        //                };
        //                _context.Addresses.Add(newAddress);
        //            }
        //        }
        //    }


        //    // --- STEP 3: Cập nhật liên hệ ---
        //    var incomingContacts = customer.Contacts;
        //    var existingContacts = existingCustomer.Contacts.ToList();

        //    // XOÁ các địa chỉ không còn trong incomingAddresses
        //    var incomingContectIds = incomingContacts.Select(a => a.ContactId).ToHashSet();

        //    foreach (var existingContact in existingContacts)
        //    {
        //        if (!incomingContectIds.Contains(existingContact.ContactId))
        //        {
        //            _context.Contacts.Remove(existingContact);
        //        }
        //    }


        //    // CẬP NHẬT hoặc THÊM mới địa chỉ
        //    foreach (var incomingContact in incomingContacts)
        //    {
        //        if (incomingContact.ContactId == Guid.Empty)
        //        {
        //            var newContact = new Contact
        //            {
        //                ContactId = Guid.CreateVersion7(),
        //                CustomerId = existingCustomer.CustomerId,
        //                FirstName = incomingContact.FirstName,
        //                LastName = incomingContact.LastName,
        //                Email = incomingContact.Email,
        //                Phone = incomingContact.Phone,
        //                IsPrimary = incomingContact.IsPrimary,
        //                Gender = incomingContact.Gender
        //            };
        //            _context.Contacts.Add(newContact);
        //        }
        //        else
        //        {
        //            var existingContact = existingContacts
        //                .FirstOrDefault(c => c.ContactId == incomingContact.ContactId);

        //            if (existingContact != null)
        //            {
        //                existingContact.FirstName = incomingContact.FirstName;
        //                existingContact.LastName = incomingContact.LastName;
        //                existingContact.Email = incomingContact.Email;
        //                existingContact.Phone = incomingContact.Phone;
        //                existingContact.IsPrimary = incomingContact.IsPrimary;
        //                existingContact.Gender = incomingContact.Gender;
        //            }
        //            else
        //            {
        //                var newContact = new Contact
        //                {
        //                    ContactId = incomingContact.ContactId,
        //                    FirstName = incomingContact.FirstName,
        //                    LastName = incomingContact.LastName,
        //                    Email = incomingContact.Email,
        //                    Phone = incomingContact.Phone,
        //                    IsPrimary = incomingContact.IsPrimary,
        //                    Gender = incomingContact.Gender
        //                };
        //                _context.Contacts.Add(newContact);
        //            }
        //        }
        //    }


        //    //await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task AddAddressAsync(List<Address> addedAddresses, CancellationToken ct)
        {
            await _context.AddRangeAsync(addedAddresses, ct);
        }

        public async Task AddContactAsync(List<Contact> addedContacts, CancellationToken ct)
        {
            await _context.AddRangeAsync(addedContacts, ct);
        }
    }
}
