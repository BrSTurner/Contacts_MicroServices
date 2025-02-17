using FIAP.DatabaseManagement.Contacts.Queries;
using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.DatabaseManagement.Context;
using FIAP.DatabaseManagement.UoW;
using FIAP.SharedKernel.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.DatabaseManagement.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool useInMemory = false)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactQueries, ContactQueries>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            if(useInMemory)
                services.AddDbContext<FIAPContext>(c => c.UseInMemoryDatabase("FIAP_Contacts"));
            else
                services.AddDbContext<FIAPContext>(c => c.UseNpgsql(configuration["POSTGRES_CONNECTION_STRING"]));

            return services;
        }
    }
}
