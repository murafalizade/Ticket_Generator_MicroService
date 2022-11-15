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
        private readonly IMongoCollection<Event> _events;

        public TicketService(ITicketDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tickets = database.GetCollection<Ticket>(settings.BooksCollectionName);
            _events = database.GetCollection<Event>("Events");
        }

        public async Task ConfirmTicket(string id)
        {
            Ticket ticket = await _tickets.Find(p => p.Id == id).FirstOrDefaultAsync();
            ticket.IsConfirmed = true;
            await _tickets.ReplaceOneAsync(x => x.Id == id, ticket);
        }

        public async Task<byte[]> CreateWithDesign([Optional] string DelegateName, string EventId, byte[] b)
        {
            Ticket ticket = new Ticket() { DelegateName = DelegateName, EventId = EventId };
            await _tickets.InsertOneAsync(ticket);
            string path = CreateQR(ticket.Id);
            string qrCodePath = CreateQR(ticket.Id);
            using (Stream inputImageStream = new FileStream(qrCodePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                MemoryStream ms = new MemoryStream();
                var reader = new PdfReader(b);
                var stamper = new PdfStamper(reader, ms);
                var pdfContentByte = stamper.GetOverContent(1);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(305, 37);
                pdfContentByte.AddImage(image);
                stamper.Close();
                return ms.ToArray();
            }
        }

        public async Task<byte[]> CreateWithoutDesign([Optional] string DelegateName, string EventId)
        {
            Ticket ticket = new Ticket() { DelegateName = DelegateName, EventId = EventId };
            await _tickets.InsertOneAsync(ticket);
            string path = CreateQR(ticket.Id);
            using (Stream inputPdfStream = new FileStream("EwA_Ticket_Design.pdf", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return CreateTicket(inputPdfStream,path);
            }
        }

        public async Task<Ticket> ValidateTicket(string id)
        {
            Ticket ticket = await _tickets.Find(p => p.Id == id).FirstOrDefaultAsync();
            Event eventt = await _events.Find(p => p.Id == ticket.EventId).FirstOrDefaultAsync();
            if (ticket == null || eventt == null)
            {
                return null;
            }
            return ticket;
        }

        private byte[] CreateTicket(Stream inputPdfStream,string qrCodePath)
        {
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
