using Notebooks.Api.HostedServices;
using Notebooks.Infra;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

services.AddHostedService<DbMigrationRunnerHostedService>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

CompositionRoot.Register(services, configuration);

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace Notebooks.Api
{
    public class Program
    {
    }
}