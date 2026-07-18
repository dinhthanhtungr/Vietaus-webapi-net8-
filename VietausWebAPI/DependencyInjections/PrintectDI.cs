using VietausWebAPI.Core.Application.Features.PrintectFeatures.RepositoryContracts;
using VietausWebAPI.Infrastructure.Repositories.Printects;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class PrintectDI
    {
        public static IServiceCollection AddPrintectModule(this IServiceCollection services)
        {
            services.AddScoped<ILabelTemplateRepository, LabelTemplateRepositories>();
            services.AddScoped<ILabelElementRepository, LabelElementRepositories>();
            services.AddScoped<IHistoryPrintLabelForAllRepository, HistoryPrintLabelForAllRepositories>();
            return services;
        }
    }
}
