using Microsoft.AspNetCore.Http;

namespace QRTicketGenerator.API.Services
{
    public interface IImageService
    {
        public string UploadImage(IFormFile file);
        public void PdfConverter(byte[] file, string outputPath);
    }
}
