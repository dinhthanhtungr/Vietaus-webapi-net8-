using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.ProductFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.Services.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Infrastructure.Repositories.Devandqas;
using VietausWebAPI.Infrastructure.Repositories.Labs.FormulaFeatures;
//using VietausWebAPI.Infrastructure.Repositories.Labs.QAQC;
//using VietausWebAPI.Infrastructure.Repositories.Labs.QAQCFeature;
using VietausWebAPI.Infrastructure.Repositories.Manufacturing;
using VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class LabsDI
    {
        public static IServiceCollection AddLabsModule(this IServiceCollection services)
        {
            // Repos
            //services.AddScoped<IProductStandardRepository, ProductStandardRepository>();
            //services.AddScoped<IProductTestRepository, ProductTestRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IManufacturingFormulaVersionRepository, ManufacturingFormulaVersionRepository>();
            services.AddScoped<IFormulaRepository, FormulaRepository>();
            services.AddScoped<IFormulaMaterialRepository, FormulaMaterialRepository>();
            services.AddScoped<ISampleRequestRepository, SampleRequestRepository>();
            services.AddScoped<IManufacturingVUFormulaRepository, ManufacturingVUFormulaRepository>();

            // Services
            //services.AddScoped<IProductStandardService, ProductStandardService>();
            //services.AddScoped<IProductInspectionService, ProductInspectionService>();
            //services.AddScoped<IProductTestService, ProductTestService>();
            services.AddScoped<IFormulaService, FormulaService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISampleRequestService, SampleRequestService>();
            services.AddScoped<IManufacturingVUFormulaService, ManufacturingVUFormulaService>();
            //services.AddScoped<IQCOutputService, QCOutputService>();

            //Helpers
            services.AddScoped<IFormulaPDF, FormulaPDF>();
            services.AddScoped<IFormulaXML, FormulaXML>();
            services.AddScoped<IVUFormulaPDF, VUFormulaPDF>();
            return services;
        }
    }
}
