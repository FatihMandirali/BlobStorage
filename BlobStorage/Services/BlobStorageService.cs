using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorage.Dtos;

namespace BlobStorage.Services;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string ContainerName = "container";

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    
    public async Task UploadAsync(Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        await containerClient.CreateIfNotExistsAsync();

        await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(fileStream,true);
    }
    
    public async Task<Stream> DownloadAsync(FileNameRequest request)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        var blobClient = containerClient.GetBlobClient(request.FileName);

        var info = await blobClient.DownloadAsync();

        return info.Value.Content;
    }
    
    public async Task DeleteAsync(FileNameRequest request)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        var blobClient = containerClient.GetBlobClient(request.FileName);

        await blobClient.DeleteAsync();
    }

}