using Azure.Storage.Blobs;
using Notebooks.Domain.Services.BinaryStorageService;
using Notebooks.Domain.Services.BinaryStorageService.Models;

namespace Notebooks.Infra.Services.BinaryStorageService;

public class AzureBlobService : IBinaryStorageService
{
    private const string ContainerName = "notebooks";
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobService(BlobServiceClient blobServiceClient)
    {
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        _blobContainerClient.CreateIfNotExists();
    }


    public async Task<PutDocumentResponse> PutContentAsync(string path, string content, CancellationToken cancellationToken)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(path);

        await using Stream stream = await blobClient.OpenWriteAsync(true, cancellationToken: cancellationToken);
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();

        PutDocumentResponse response = new(blobClient.Uri);
        return response;
    }

    public async Task<string> ReadContentAsync(string path, CancellationToken cancellationToken)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(path);
        await using Stream stream = await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        return content;
    }
}