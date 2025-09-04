using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Features.HR.Services;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Planning.Services;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.DataUnitOfWork;
using VietausWebAPI.Infrastructure.Repositories.HR;
using VietausWebAPI.Infrastructure.Repositories.Labs.QAQC;
using VietausWebAPI.Infrastructure.Repositories.Labs;
using VietausWebAPI.Infrastructure.Repositories.Planning.Schedueal;
using VietausWebAPI.Infrastructure.Repositories.Sales;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Infrastructure.Repositories.Labs.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.ImageStorage;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Infrastructure.Repositories.Materials;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Services;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.QAQCFeatures;

namespace VietausWebAPI.WebAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            // HR
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMemberInGroupRepository, MemberInGroupRepository>();

            // Sale
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITransferCustomerService, TransferCustomerService>();
            services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();
            services.AddScoped<ICustomerTransferLogRepository, CustomerTransferLogRepository>();
            services.AddScoped<ICustomerAssignmentRepository, CustomerAssignmentRepository>();
            //services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();
            //services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();

            //Materials
            //services.AddScoped<IMaterialsRepository, MaterialsRepository>();
            //services.AddScoped<IMaterialsService, MaterialsService>();

            //Labs
            //ProductStandard
            services.AddScoped<IProductStandardRepository, ProductStandardRepository>();

            services.AddScoped<IProductStandardService, ProductStandardService>();
            //ProductInspection
            services.AddScoped<IProductInspectionRepository, ProductInspectionRepository>();
            services.AddScoped<IProductInspectionService, ProductInspectionService>();
            //ProductTest
            services.AddScoped<IProductTestRepository, ProductTestRepository>();
            services.AddScoped<IProductTestService, ProductTestService>();
            //MfgProductionOrdersPlanRepository
            services.AddScoped<IMfgProductionOrdersPlanRepository, MfgProductionOrdersPlanRepository>();

            //SampleRequest
            services.AddScoped<ISampleRequestService, SampleRequestService>();
            services.AddScoped<ISampleRequestRepository, SampleRequestRepository>();

            //SampleRequestImage
            services.AddScoped<ISampleRequestImageService, SampleRequestImageService>();
            services.AddScoped<ISampleRequestImageRepository, SampleRequestImageRepository>();
            services.AddScoped<IFileStorage, LocalFileStorage>();    

            //Product
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            //QC 
            services.AddScoped<IQCDetailRepository, QCDetailRepository>();
            services.AddScoped<IQCOutputService, QCOutputService>();

            //Planning
            //Schedual
            services.AddScoped<IScheduealService, ScheduealService>();
            services.AddScoped<IScheduealRepository, ScheduealRepository>();


            //Material
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IMaterialRepository, MaterialsRepository>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMaterialsSupplierRepository, MaterialsSupplierRepository>();
            services.AddScoped<IPriceHistorieRepository, PriceHistorieRepository>();

            //services.AddScoped<IMaterialService, MaterialService>

            return services;
        }
    }
}
