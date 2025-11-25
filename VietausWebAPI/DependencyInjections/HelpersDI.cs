using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.PriceHelpers;
using VietausWebAPI.Infrastructure.Helpers.IdCounter;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class HelpersDI
    {
        public static IServiceCollection AddHelpersModule(this IServiceCollection services)
        {
            services.AddScoped<IExternalIdService, ExternalIdServicePostgres>();
            services.AddScoped<IPriceProvider, MaterialsSupplierPriceProvider>();
            return services;
        }
    }
}
