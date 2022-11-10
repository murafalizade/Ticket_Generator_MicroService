using iTextSharp.text;
using iTextSharp.text.pdf;
using MongoDB.Driver;
using QRCoder;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.API.Models;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketService(ITicketDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tickets = database.GetCollection<Ticket>(settings.BooksCollectionName);
        }

        public Task ConfirmTicket(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateWithDesign()
        {
            throw new System.NotImplementedException();
        }

        public async Task<byte[]> CreateWithoutDesign([Optional] string DelegateName, int EventId)
        {
            Ticket ticket = new Ticket() { DelegateName = DelegateName, EventId = EventId };
             await _tickets.InsertOneAsync(ticket);
            string path = CreateQR(ticket.Id);
            return CreateTicket(path);
        }

        public async Task<Ticket> ValidateTicket(string id)
        {
           Ticket ticket = await _tickets.Find(p=>p.Id == id).FirstOrDefaultAsync();
            if (ticket == null)
            {
                return null;
            }
            return ticket;
        }

        private byte[] CreateTicket(string qrCodePath)
        {
            using (Stream inputPdfStream = new FileStream("EwA_Ticket_Design.pdf", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new FileStream(qrCodePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                MemoryStream ms = new MemoryStream();
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, ms);
                var pdfContentByte = stamper.GetOverContent(1);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(305, 37);
                pdfContentByte.AddImage(image);
                stamper.Close();
                return ms.ToArray();
            }
        }

        private string CreateQR(string ticketId)
        {
            string qrCodePath = $"qr/qr_code.{ticketId}.jpg";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://localhost:44339/api/TicketController/{ticketId}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);
            qrCodeImage.Save(qrCodePath);
            return qrCodePath;
        }
    }
}
