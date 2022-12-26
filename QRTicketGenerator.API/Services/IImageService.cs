using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public interface IImageService
    {
        public byte[] GetImageAsync(string imagePath, string containerName);
        //public string UploadImage(IFormFile file)
        public Task<string> UploadImage(IFormFile file, string containerName);
    }
}
