using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] GetImageAsync(string url, string containerName = "uploads")
        {
            string connectionString = _configuration.GetSection("AzureStorage:ConnectionString").Value;
            BlobClient blobClient = new BlobClient(connectionString,containerName,url);
            var ms = new MemoryStream();
            blobClient.DownloadTo(ms);
            return ms.ToArray();
        }   
        
        //public string UploadImage(IFormFile file)
        //{
        //    string fileName = Guid.NewGuid().ToString() + ".pdf";
        //    string filePath = "uploads/" + fileName;
        //    using (var ms = new MemoryStream())
        //    {
        //        file.CopyTo(ms);
        //        PdfConverter(ms.ToArray(), filePath);
        //    }
        //    return filePath;
        //}

        public async Task<string> UploadImage(IFormFile file, string containerName = "uploads")
        {  
            string fileName = $"{Guid.NewGuid().ToString()}{System.IO.Path.GetExtension(file.FileName)}";
            string connectionString = _configuration.GetSection("AzureStorage:ConnectionString").Value;
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Position = 0;
            BlobClient blobClient = container.GetBlobClient(fileName);
            await blobClient.UploadAsync(ms);
            return blobClient.Uri.ToString();
        }
    }
}
