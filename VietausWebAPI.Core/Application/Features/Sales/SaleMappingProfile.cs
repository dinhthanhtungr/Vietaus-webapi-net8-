using AutoMapper;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Sales
{
    public class SaleMappingProfile : Profile
    {
        public SaleMappingProfile()
        {
            // Address
            CreateMap<PostAddress, Address>().ReverseMap();
            CreateMap<GetAddress, Address>().ReverseMap();

            // Contact
            CreateMap<PostContact, Contact>().ReverseMap();
            CreateMap<GetContact, Contact>().ReverseMap();

            // ---- CustomerAssignment: BẮT BUỘC phải có map phần tử ----
            CreateMap<PostCustomerAssignment, CustomerAssignment>();

            // Customer: Entity -> DTO
            CreateMap<Customer, GetCustomer>()
                .ForMember(d => d.Addresses, opt => opt.MapFrom(s => s.Addresses))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(s => s.Contacts))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name));
            // (Nếu cần chiều ngược lại:) CreateMap<GetCustomer, Customer1>(); // tùy nhu cầu

            // Customer: DTO -> Entity
            CreateMap<PostCustomer, Customer>()
                .ForMember(d => d.CustomerId, opt => opt.Ignore())
                .ForMember(d => d.Addresses, opt => opt.MapFrom(s => s.Addresses))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(s => s.Contacts))
                // Nguồn: 1 object -> Đích: collection
                .ForMember(d => d.CustomerAssignments, opt => opt.MapFrom(s =>
                    s.CustomerAssignment != null
                        ? new[] { s.CustomerAssignment }
                        : Array.Empty<PostCustomerAssignment>()))


                // Nếu DTO không gửi IsActive, set mặc định:
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                // ⬇️ không nhận thời gian từ client
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.UpdatedDate, o => o.Ignore())
                .ForMember(d => d.IssueDate, o => o.Ignore());

            // Review DTO
            CreateMap<Customer, GetReviewCustomer>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName))
                .ForMember(d => d.RegNo, opt => opt.MapFrom(s => s.RegistrationNumber))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Phone))
                .ForMember(d => d.Group, opt => opt.MapFrom(s => s.CustomerGroup))

                // DeliveryName: ưu tiên contact main, không có main thì lấy contact đầu
                .ForMember(d => d.DeliveryName, o => o.MapFrom(s =>
                    s.Contacts != null && s.Contacts.Any()
                        ? s.Contacts
                            .OrderByDescending(c => c.IsPrimary) // nếu dùng IsPrimary/IsDefault thì đổi tên field
                            .Select(c => ((c.FirstName ?? "") + " " + (c.LastName ?? "")).Trim())
                            .FirstOrDefault()
                        : null
                ))

                // Address: cũng ưu tiên contact main, không có main thì lấy địa chỉ của contact đầu
                .ForMember(d => d.Address, o => o.MapFrom(s =>
                    s.Addresses != null && s.Addresses.Any()
                        ? s.Addresses
                            .OrderByDescending(c => c.IsPrimary)
                            .Select(c => c.AddressLine) // nếu địa chỉ nằm ở field khác thì đổi lại
                            .FirstOrDefault()
                        : null
                ))

                // EmployeeId: Lấy EmployeeId từ CustomerAssignments nếu có
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.CustomerAssignments
                    .Where(a => a.IsActive)
                    .Select(a => (Guid?)a.EmployeeId)
                    .FirstOrDefault()))

                // EmployeeName: Lấy tên nhân viên từ CustomerAssignments, dựa vào EmployeeId đã lấy ở trên
                .ForMember(d => d.EmployeeName, opt => opt.MapFrom((s, d) =>
                    s.CustomerAssignments
                        .Where(a => a.IsActive && a.EmployeeId == d.EmployeeId) // Lọc theo EmployeeId
                        .Select(a => a.Employee.FullName)
                        .FirstOrDefault()));


            // Patch
            CreateMap<PatchCustomer, Customer>().ReverseMap();

            // Map wrapper PagedResult<Src> -> PagedResult<Dest>
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

            // MerchadiseOrder
            CreateMap<PostMerchandiseOrderDetail, MerchandiseOrderDetail>().ReverseMap();
            CreateMap<MerchandiseOrderDetail, GetMerchandiseOrderDetail>().ReverseMap();

            CreateMap<PostMerchandiseOrder, MerchandiseOrder>()
                .ForMember(d => d.ExternalId, opt => opt.Ignore())
                .ForMember(d => d.MerchandiseOrderDetails, opt => opt.MapFrom(s => s.merchandiseOrderDetails))
                // Nếu DTO không gửi IsActive, set mặc định:
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                // ⬇️ không nhận thời gian từ client
                .ForMember(d => d.CreateDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.UpdatedDate, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<MerchandiseOrder, GetMerchadiseOrder>()
                .ForMember(d => d.merchandiseOrderDetails, opt => opt.MapFrom(s => s.MerchandiseOrderDetails.Where(x => x.IsActive == true)));
            
            CreateMap<MerchandiseOrder, GetMerchadiseOrderWithId>()
                .ForMember(d => d.merchandiseOrderDetails, opt => opt.MapFrom(s => s.MerchandiseOrderDetails.Where(x => x.IsActive == true)));

            CreateMap<MerchandiseOrder, GetOldProductInformation>();

            // OrderAttachment

            //CreateMap<OrderAttachment, OrderAttachmentDTO>().ReverseMap();


            // Mapper cho phần tạo lệnh sản xuất kèm theo đơn hàng bán
            CreateMap<PostMfgProductionOrder, MfgProductionOrder>()
                // chỉ map những field được phép từ client:
                .ForMember(d => d.MerchandiseOrderId, o => o.MapFrom(s => s.MerchandiseOrderId))
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
                .ForMember(d => d.CustomerId, o => o.MapFrom(s => s.CustomerId))
                .ForMember(d => d.ManufacturingDate, o => o.MapFrom(s => s.ManufacturingDate))
                .ForMember(d => d.ExpectedDate, o => o.MapFrom(s => s.ExpectedDate))
                .ForMember(d => d.requiredDate, o => o.MapFrom(s => s.requiredDate))
                .ForMember(d => d.TotalQuantity, o => o.MapFrom(s => s.TotalQuantity))
                .ForMember(d => d.NumOfBatches, o => o.MapFrom(s => s.NumOfBatches))
                .ForMember(d => d.Requirement, o => o.MapFrom(s => s.Requirement))
                .ForMember(d => d.BagType, o => o.MapFrom(s => s.BagType))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.CreateDate, o => o.Ignore())         // server set
                .ForMember(d => d.ExternalId, o => o.Ignore())         // server gen
                .ForMember(d => d.Status, o => o.Ignore())         // server set
                .ForMember(d => d.CompanyId, o => o.Ignore())         // lấy từ token
                .ForMember(d => d.IsActive, o => o.Ignore())
                .ForMember(d => d.QcCheck, o => o.Ignore())
                //.ForMember(d => d.QualifiedQuantity, o => o.Ignore())
                //.ForMember(d => d.RejectedQuantity, o => o.Ignore())
                //.ForMember(d => d.WasteQuantity, o => o.Ignore())
                // tất cả snapshot phải do server fill, không lấy từ client:
                .ForMember(d => d.ProductExternalIdSnapshot, o => o.Ignore())
                .ForMember(d => d.ProductNameSnapshot, o => o.Ignore())
                .ForMember(d => d.CustomerExternalIdSnapshot, o => o.Ignore())
                .ForMember(d => d.CustomerNameSnapshot, o => o.Ignore())
                .ForMember(d => d.FormulaId, o => o.Ignore())     // bạn set sau khi lookup
                .ForMember(d => d.FormulaExternalIdSnapshot, o => o.Ignore())
                // nav props luôn Ignore khi map từ DTO
                .ForMember(d => d.Product, o => o.Ignore())
                .ForMember(d => d.Customer, o => o.Ignore())
                .ForMember(d => d.Company, o => o.Ignore())
                .ForMember(d => d.CreatedByNavigation, o => o.Ignore())
                .ForMember(d => d.UpdatedByNavigation, o => o.Ignore())
                .ForMember(d => d.ManufacturingFormulas, o => o.Ignore());
        }
    }
}
