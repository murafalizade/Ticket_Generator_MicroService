using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

class AzureStorageService
{
    private static async Task<BlobContainerClient> CreateContainerAsync(BlobServiceClient blobServiceClient)
    {
        // Name the sample container based on new GUID to ensure uniqueness.
        // The container name must be lowercase.
        string containerName = "uploads";

        try
        {
            // Create the container
            BlobContainerClient container = await blobServiceClient.CreateBlobContainerAsync(containerName);

            if (await container.ExistsAsync())
            {
                Console.WriteLine("Created container {0}", container.Name);
                return container;
            }
        }
        catch (RequestFailedException e)
        {
            Console.WriteLine("HTTP error code {0}: {1}",
                                e.Status, e.ErrorCode);
            Console.WriteLine(e.Message);
        }

        return null;
    }

    public static async Task<string> UploadBinary (BlobContainerClient containerClient, byte[] buffer, string fileName)
    {
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        BinaryData binaryData = new BinaryData(buffer);
        await blobClient.UploadAsync(binaryData, true);
        return blobClient.Uri.ToString();
    }

    public static async Task<Stream> DownloadFile (BlobContainerClient containerClient, string fileName)
    {
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        BlobDownloadInfo download = await blobClient.DownloadAsync();
        return download.Content;
    }
}