using Contacts_Manager.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Repositories;
using Service;
using ServiceContracts;

namespace Contacts_Manager
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // add Services to IOC container
            // Singleton: Instantiate one time for all the application
            services.AddScoped<ICountriesRepository, CountriesRepositories>();
            services.AddScoped<IPersonsRepository, PersonsRepositories>();

            services.AddScoped<ICountryService, CountryService>();
            // 
            services.AddScoped<IPersonService, PersonService>();

            // For IFilterFactory
            services.AddTransient<ResponseHeaderActionFilter>();

            // DbContext Registeration
            services.AddDbContext<PersonsDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:CMConnection"]);

                options.UseSqlServer(configuration.GetConnectionString("CMConnection"));
            });

            // For Logging Specific Fields
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });
            return services;
        }
    }
}
