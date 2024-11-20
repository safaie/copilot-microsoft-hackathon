using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's service registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ServiceDescriptor)); // Fix type to ServiceDescriptor

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add the test service registration
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        });
    }
}