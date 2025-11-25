using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures
{
    public class MaterialMappingProfile : Profile
    {
        public MaterialMappingProfile()
        {
            // Address
            CreateMap<PostSupplierAddress, SupplierAddress>().ReverseMap();
            CreateMap<GetSupplierAddress, SupplierAddress>().ReverseMap();

            // Contact
            CreateMap<PostSupplierContact, SupplierContact>().ReverseMap();
            CreateMap<GetSupplierContact, SupplierContact>().ReverseMap();


            // Supplier: Entity -> DTO
            CreateMap<Supplier, GetSupplier>()
                    .ForMember(d => d.SupplierAddresses, opt => opt.MapFrom(s => s.SupplierAddresses))
                    .ForMember(d => d.SupplierContacts, opt => opt.MapFrom(s => s.SupplierContacts))
                    .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name));
            // (Nếu cần chiều ngược lại:) CreateMap<GetCustomer, Customer1>(); // tùy nhu cầu


            // Supplier: DTO -> Entity
            CreateMap<PostSupplier, Supplier>()
                .ForMember(d => d.SupplierId, opt => opt.Ignore())
                .ForMember(d => d.SupplierAddresses, opt => opt.MapFrom(s => s.SupplierAddresses))
                .ForMember(d => d.SupplierContacts, opt => opt.MapFrom(s => s.SupplierContacts))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.UpdatedDate, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Supplier, GetSupplierSummary>()
                .ForMember(d => d.Phone, opt => opt.MapFrom(s =>
                    s.SupplierContacts
                     .Where(c => c.IsPrimary == true)
                     .Select(c => c.Phone)
                     .FirstOrDefault()))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s =>
                    s.SupplierContacts
                     .Where(c => c.IsPrimary == true)
                     .Select(c => c.FirstName)
                     .FirstOrDefault()))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s =>
                    s.SupplierContacts
                     .Where(c => c.IsPrimary == true)
                     .Select(c => c.LastName)
                     .FirstOrDefault()));


            // Material summary
            CreateMap<Material, GetMaterialSummary>()
                .ForMember(d => d.Category, opt => opt.MapFrom(s => $"{s.Category.Name}"))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.MaterialsSuppliers
                                                        .OrderByDescending(ms => ms.UpdatedDate)
                                                        .Select(ms => (decimal?)ms.CurrentPrice)
                                                        .FirstOrDefault()));

            CreateMap<Material, GetMaterial>();



            //PriceHistory
            //CreateMap<PostPriceHistory, PriceHistory>()
            //    .ForMember(d => d.PriceHistoryId, o => o.Ignore())        // KHÔNG cho DTO ghi đè khóa chính
            //    .ForMember(d => d.MaterialId, o => o.Ignore())           // ĐỂ EF tự set từ parent.Material.PriceHistories
            //    .ForMember(d => d.Supplier, o => o.Ignore())            // KHÔNG map navigation để tránh EF chèn Supplier mới
            //    .ForMember(d => d.IsActive, o => o.MapFrom(_ => true)) // Bản ghi giá mới tạo là "đang hiệu lực"
            //    .ForMember(d => d.CreateDate, o => o.MapFrom(_ => DateTime.Now))
            //    .ForMember(d => d.EndDate, o => o.MapFrom(_ => (DateTime?)null));



            //            MapFrom((src, dest, destMember, ctx) => { ... })

            //              src: source – chính là PostMaterial bạn đưa vào.

            //              dest: destination – đối tượng Material đang được tạo/ map tới.

            //              _: là discard(bỏ qua) cho destMember(giá trị hiện tại của PriceHistories trước khi map). Mình không dùng nên đặt _.

            //              ctx: ResolutionContext của AutoMapper – cho phép gọi lại mapper con, truy cập ctx.Items, v.v.

            //Material 
            CreateMap<PostMaterial, Material>()
                .ForMember(d => d.MaterialId, opt => opt.Ignore())
                .ForMember(d => d.MaterialsSuppliers, opt => opt.Ignore()) // ⬅️ quan trọng
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.UpdatedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true));
                //.ForMember(d => d.PriceHistories, o => o.MapFrom((src, dest, _, ctx) =>
                //src.InitialPrice == null
                //    ? new List<PriceHistory>()
                //    : new List<PriceHistory> {
                //        ctx.Mapper.Map<PriceHistory>(src.InitialPrice)
                //    }));

            //      Logic bên trong

            //            Nếu src.InitialPrice == null → trả về new List<PriceHistory>()(list rỗng).

            //            Ngược lại:

            //            Dùng ctx.Mapper.Map<PriceHistory>(src.InitialPrice) để map object con PostPrice → PriceHistory(cần có CreateMap<PostPrice, PriceHistory>() trước đó).

            //            Bọc vào new List<PriceHistory> { ... } để biến một object thành một phần tử trong collection PriceHistories.



            // Materials_Suppliers
            CreateMap<PostMaterialSupplier, MaterialsSupplier>()
                .ForMember(d => d.MaterialsSuppliersId, o => o.Ignore())
                .ForMember(d => d.MaterialId, o => o.Ignore())
                .ForMember(d => d.UpdatedDate, o => o.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.UpdatedBy, o => o.Ignore())                
                .ForMember(d => d.CreateDate, o => o.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.CreatedBy, o => o.Ignore());

            CreateMap<MaterialsSupplier, GetMaterialSupplier>()
                .ForMember(d => d.SupplierName, o => o.MapFrom(s => s.Supplier.SupplierName))
                .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.Supplier.ExternalId));


        }
    }
}
