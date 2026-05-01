using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.ProductFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.Services.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs.ColorChipRecords;
using VietausWebAPI.Infrastructure.Repositories.Devandqas;
using VietausWebAPI.Infrastructure.Repositories.Labs.FormulaFeatures;
//using VietausWebAPI.Infrastructure.Repositories.Labs.QAQC;
//using VietausWebAPI.Infrastructure.Repositories.Labs.QAQCFeature;
using VietausWebAPI.Infrastructure.Repositories.Manufacturing;
using VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature;
using VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature.ColorChipRecordFeatures;

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
            services.AddScoped<IFormulaMaterialSnapshotRepository, FormulaMaterialSnapshotRepository>();

            services.AddScoped<IColorChipRecordReadRepositories, ColourChipRecordReadRepositories>();
            services.AddScoped<IColorChipRecordWriteRepositories, ColourChipRecordWriteRepositories>();
            services.AddScoped<IColorChipRecordUpsertRepositories, ColourChipRecordUpsertRepositories>();

            // Services
            //services.AddScoped<IProductStandardService, ProductStandardService>();
            //services.AddScoped<IProductInspectionService, ProductInspectionService>();
            //services.AddScoped<IProductTestService, ProductTestService>();
            services.AddScoped<IFormulaService, FormulaService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISampleRequestService, SampleRequestService>();
            services.AddScoped<IManufacturingVUFormulaService, ManufacturingVUFormulaService>();
            //services.AddScoped<IQCOutputService, QCOutputService>();

            services.AddScoped<IColourChipRecordWriteServices, ColourChipRecordWriteServices>();  
            services.AddScoped<IColourChipRecordReadServices, ColourChipRecordReadServices>();
            services.AddScoped<IColourChipRecordUpsertServices, ColourChipRecordUpsertServices>();
            services.AddScoped<IColourChipRecordPrintPDFService, ColourChipRecordPrintPDFService>();
            services.AddScoped<IColorChipRecordLandscapePdf, ColorChipRecordLandscapePdf>();
            services.AddScoped<IColorChipRecordPortraitPdf, ColorChipRecordPortraitPdf>();
            services.AddScoped<IColorChipRecordTanPhuPdf, ColorChipRecordTanPhuPdf>();
            services.AddScoped<IColorChipRecordTanPhuBacNinhPdf, ColorChipRecordTanPhuBacNinhPdf>();

            //Helpers
            services.AddScoped<IFormulaPDF, FormulaPDF>();
            services.AddScoped<IFormulaXML, FormulaXML>();
            services.AddScoped<IVUFormulaPDF, VUFormulaPDF>();
            services.AddScoped<IProductFormulaRuleHelper, ProductFormulaRuleHelper>();
            return services;
        }
    }
}
