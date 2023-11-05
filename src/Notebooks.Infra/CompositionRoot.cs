using Azure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notebooks.Application.NotebookStories;
using Notebooks.Application.NotebookStories.Imp;
using Notebooks.Domain.Db;
using Notebooks.Domain.Exceptions;
using Notebooks.Domain.Services.BinaryStorageService;
using Notebooks.Infra.Db;
using Notebooks.Infra.Services.BinaryStorageService;

namespace Notebooks.Infra;

public static class CompositionRoot
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGetNotebookListStory, GetNotebookListStory>();
        services.AddScoped<IGetNotebookStory, GetNotebookStory>();
        services.AddScoped<ICreateNotebookStory, CreateNotebookStory>();
        services.AddScoped<IUpdateNotebookStory, UpdateNotebookStory>();

        AzureBlobServiceConfig azureBlobServiceConfig = configuration.GetSection("AzureBlobServiceConfig").Get<AzureBlobServiceConfig>()
                                                        ?? throw new DevelopmentException("AzureBlobServiceConfig is null");
        services.AddAzureClients(builder => { builder.AddBlobServiceClient(azureBlobServiceConfig.BaseUrl, new StorageSharedKeyCredential(azureBlobServiceConfig.AccountName, azureBlobServiceConfig.Password)); });
        services.AddScoped<IBinaryStorageService, AzureBlobService>();

        var connectionStr = configuration.GetConnectionString("NotebooksDbSqlServer")
                            ?? throw new DevelopmentException("NotebooksDbSqlServer is null");

        services.AddSingleton<IDbMigrationEngine, DbMigrationEngine>(_ => new DbMigrationEngine(connectionStr));
        services.AddDbContext<IRepositoryContext, RepositoryContext>((_, builder) => { builder.UseSqlServer(connectionStr); });
    }
}