using AutoMapper;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Domain.Entities;

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
            CreateMap<Customer1, GetCustomer>()
                .ForMember(d => d.Addresses, opt => opt.MapFrom(s => s.Addresses))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(s => s.Contacts))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name));
            // (Nếu cần chiều ngược lại:) CreateMap<GetCustomer, Customer1>(); // tùy nhu cầu

            // Customer: DTO -> Entity
            CreateMap<PostCustomer, Customer1>()
                .ForMember(d => d.CustomerId, opt => opt.Ignore())
                .ForMember(d => d.Addresses, opt => opt.MapFrom(s => s.Addresses))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(s => s.Contacts))
                // Nguồn: 1 object -> Đích: collection
                .ForMember(d => d.CustomerAssignments, opt => opt.MapFrom(s =>
                    s.CustomerAssignment != null
                        ? new[] { s.CustomerAssignment }
                        : Array.Empty<PostCustomerAssignment>()))

                
                // Nếu DTO không gửi IsActive, set mặc định:
                .ForMember(d => d.IsActive, opt => opt.MapFrom(_ => true));

            // Review DTO
            CreateMap<Customer1, GetReviewCustomer>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName))
                .ForMember(d => d.RegNo, opt => opt.MapFrom(s => s.RegistrationNumber))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Phone))
                .ForMember(d => d.Group, opt => opt.MapFrom(s => s.CustomerGroup))
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.CustomerAssignments
                                                                            .Where(a => a.IsActive)
                                                                            .Select(a => (Guid?)a.EmployeeId)
                                                                            .FirstOrDefault()))
                .ForMember(d => d.EmployeeName, opt => opt.MapFrom(s => s.CustomerAssignments
                                                                            .Where(a => a.IsActive)
                                                                            .Select(a => a.Employee.FullName)
                                                                            .FirstOrDefault()));

                

            // Patch
            CreateMap<PatchCustomer, Customer1>().ReverseMap();
        }
    }
}
