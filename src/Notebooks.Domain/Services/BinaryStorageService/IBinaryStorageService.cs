using Notebooks.Domain.Services.BinaryStorageService.Models;

namespace Notebooks.Domain.Services.BinaryStorageService;

public interface IBinaryStorageService
{
    Task<PutDocumentResponse> PutContentAsync(string path, string content, CancellationToken cancellationToken);
    Task<string> ReadContentAsync(string path, CancellationToken cancellationToken);
}