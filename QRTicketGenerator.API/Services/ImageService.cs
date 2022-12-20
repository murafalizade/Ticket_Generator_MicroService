using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;

namespace QRTicketGenerator.API.Services
{
    public class ImageService : IImageService
    {
        public string UploadImage(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + ".pdf";
            string filePath = "uploads/" + fileName;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                PdfConverter(ms.ToArray(), filePath);
            }
            return filePath;
        }

        public void PdfConverter(byte[] file, string outputPath)
        {
            iTextSharp.text.Rectangle pageSize = null;
            MemoryStream ms = new MemoryStream();
            MemoryStream imageStream = new MemoryStream(file);
            using (var image = new Bitmap(imageStream))
            {
                pageSize = new iTextSharp.text.Rectangle(0, 0, image.Width, image.Height);
            }
            var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
            iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
            document.Open();
            var imageProps = iTextSharp.text.Image.GetInstance(file);
            document.Add(imageProps);
            document.Close();
            File.WriteAllBytes(outputPath, ms.ToArray());
        }
    }
}
