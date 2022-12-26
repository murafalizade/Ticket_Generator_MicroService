using iTextSharp.text;
using iTextSharp.text.pdf;
using MongoDB.Driver;
using QRCoder;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;
        private readonly IMongoCollection<Event> _events;
        private readonly IMongoCollection<TicketDesign> _ticketDesigns;
        private readonly IMongoCollection<User> _users;
        private readonly IImageService _imageService;

        public TicketService(ITicketDatabaseSettings settings, IImageService imageService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("Users");
            _tickets = database.GetCollection<Ticket>(settings.BooksCollectionName);
            _events = database.GetCollection<Event>("Events");
            _ticketDesigns = database.GetCollection<TicketDesign>("TicketDeisgns");
            _imageService = imageService;
        }

        public async Task ConfirmTicket(string id)
        {
            Ticket ticket = await _tickets.Find(p => p.Id == id).FirstOrDefaultAsync();
            ticket.IsConfirmed = true;
            await _tickets.ReplaceOneAsync(x => x.Id == id, ticket);
        }
        public async Task<byte[]> CreateTicketFile(TicketAttributeDto ticket, string userId)
        {
            // Create event for decreasing coin count in microservice in rabbitmq
            //string userId = "3b2d5ef9-8e89-492f-9919-b6cf273255ec";
            User user = await _users.Find(p => p.Id == userId).FirstOrDefaultAsync();
            Event e = await _events.Find(p => p.Id == ticket.EventId).FirstOrDefaultAsync();
            if (user == null || user.CoinCount == 0 || e == null)
            {
                return null;
            }
            Ticket tickets = new Ticket() { DelegateName = ticket.DelegateName, EventId = ticket.EventId };
            await _tickets.InsertOneAsync(tickets);
            byte[] qr = CreateQR(tickets.Id);
            TicketDesign ticketDesign = await _ticketDesigns.Find(p => p.Id == ticket.TicketDesignId).FirstOrDefaultAsync();
            if (ticketDesign == null)
            {
                return null;
            }
            byte[] designImage = _imageService.GetImageAsync(ticketDesign.DesignFilePath, "uploads");
            designImage = PdfConverter(designImage);
            user.CoinCount--;
            await _users.ReplaceOneAsync(x => x.Id == userId, user);
            return CreateTicket(designImage, qr, ticketDesign, ticket);

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

        private byte[] CreateTicket(byte[] inputPdfStream, byte[] qrCodePath, TicketDesign ticketDesign, TicketAttributeDto ticketValue)
        {
            MemoryStream ms = new MemoryStream();
            var reader = new PdfReader(inputPdfStream);
            var stamper = new PdfStamper(reader, ms);
            var pdfContentByte = stamper.GetOverContent(1);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(qrCodePath);
            image.SetAbsolutePosition(ticketDesign.QrCodeX, ticketDesign.QrCodeY);
            pdfContentByte.AddImage(image);

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            // Value 1
            pdfContentByte.SetFontAndSize(bf, ticketDesign.fontSize1);
            pdfContentByte.SetColorFill(BaseColor.Black);
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(1, ticketValue.Value1, ticketDesign.PositionX1, ticketDesign.PositionY1, 0);
            pdfContentByte.SetColorFill(BaseColor.Black);
            // Value 2
            pdfContentByte.SetFontAndSize(bf, ticketDesign.fontSize2);
            pdfContentByte.SetColorFill(BaseColor.Black);
            pdfContentByte.ShowTextAligned(1, ticketValue.Value2, ticketDesign.PositionX2, ticketDesign.PositionY2, 0);
            // Value 3
            pdfContentByte.SetFontAndSize(bf, ticketDesign.fontSize3);
            pdfContentByte.ShowTextAligned(1, ticketValue.Value3, ticketDesign.PositionX3, ticketDesign.PositionY3, 0);
            pdfContentByte.SetColorFill(BaseColor.Black);
            // Value 4
            pdfContentByte.SetFontAndSize(bf, ticketDesign.fontSize4);
            pdfContentByte.ShowTextAligned(1, ticketValue.Value4, ticketDesign.PositionX4, ticketDesign.PositionY4, 0);

            pdfContentByte.EndText();
            stamper.Close();
            return ms.ToArray();
        }

        private byte[] PdfConverter(byte[] file)
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
            return ms.ToArray();
        }

        private byte[] CreateQR(string ticketId)
        {
            string qrCodePath = $"qr/qr_code.{ticketId}.jpg";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://localhost:5001/api/TicketController/{ticketId}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);
            using (var ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
