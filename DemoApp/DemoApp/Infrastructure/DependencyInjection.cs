namespace DemoApp.Infrastructure
{
    using System.Reflection;
    using DemoApp.Application.Abstractions;
    using DemoApp.Application.Mapping;
    using DemoApp.Infrastructure.Persistence;
    using DemoApp.Infrastructure.Services;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Dependency injection registration helpers.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application and infrastructure services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);
            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            var section = configuration.GetSection("ExchangeRate");
            var baseUrl = section.GetValue<string>("BaseUrl")?.TrimEnd('/') ?? "https://api.frankfurter.app";
            services.AddHttpClient("exchangerate", client =>
            {
                client.BaseAddress = new Uri(baseUrl + "/");
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            });
            return services;
        }
    }
}
