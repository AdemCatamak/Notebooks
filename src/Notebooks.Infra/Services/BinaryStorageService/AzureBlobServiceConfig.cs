namespace Notebooks.Infra.Services.BinaryStorageService;

public record AzureBlobServiceConfig(Uri BaseUrl, string AccountName, string Password);