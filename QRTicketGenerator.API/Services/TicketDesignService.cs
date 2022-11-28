using AutoMapper;
using MongoDB.Driver;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.Shared;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public class TicketDesignService : ITicketDesignService
    {
        private readonly IMongoCollection<TicketDesign> _ticketDesigns;
        private readonly IMapper _mapper;
        public TicketDesignService(ITicketDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _ticketDesigns = database.GetCollection<TicketDesign>("TicketDeisgns");
            _mapper = mapper;
        }
        public async Task<ResponseDto<string>> Create(TicketDesignDto entity, string userId)
        {
            if (userId == null)
            {
                return ResponseDto<string>.Fail("User not found", 401);
            }
            TicketDesign ticketDesign = _mapper.Map<TicketDesign>(entity);
            ticketDesign.Userid = userId;
            await _ticketDesigns.InsertOneAsync(ticketDesign);
            return ResponseDto<string>.Success(ticketDesign.Id,200);
        }

        public async Task<ResponseDto<List<TicketDesign>>> GetAll(string userId)
        {
            if(userId == null)
            {
                return  ResponseDto<List<TicketDesign>>.Fail("User not found",401);
            }
            List<TicketDesign> ticketDesigns = await _ticketDesigns.Find(x=> x.Userid == userId).ToListAsync();

                return  ResponseDto<List<TicketDesign>>.Success(ticketDesigns,200);

        }

        public async Task<ResponseDto<TicketDesign>> GetById(string id, string userId)
        {
            if (id == null)
            {
                return ResponseDto<TicketDesign>.Fail("Event Id is null", 403);
            }
            if (userId == null)
            {
                return ResponseDto<TicketDesign>.Fail("User is null", 401);
            }
            TicketDesign ticketDesign = await _ticketDesigns.Find(x => x.Id == id && x.Userid == userId).FirstOrDefaultAsync();
            return ResponseDto<TicketDesign>.Success(ticketDesign, 200);
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

        public async Task<ResponseDto<NoContent>> Delete(string id, string userId)
        {
            if (id == null)
            {
                return ResponseDto<NoContent>.Fail("Event Id is null", 403);
            }
            if (userId == null)
            {
                return ResponseDto<NoContent>.Fail("User is null", 401);
            }
            await _ticketDesigns.DeleteOneAsync(x => x.Userid == userId && x.Id == id);
            return ResponseDto<NoContent>.Success(new NoContent(),204);
        }

        public async Task<ResponseDto<NoContent>> Update(UpdateTicketDesignDto entity, string userId)
        {
            if (userId == null)
            {
                return ResponseDto<NoContent>.Fail("User is null", 401);
            }
            var ticketDesign = await GetById(entity.Id, userId);
            if (ticketDesign == null)
            {
                return ResponseDto<NoContent>.Fail("Ticket design not found", 404);
            }
            string path = ticketDesign.Data.DesignFilePath;
            File.Delete(path);
            TicketDesign ticketDesign1 = _mapper.Map<TicketDesign>(entity);
            await _ticketDesigns.ReplaceOneAsync(x => x.Id == ticketDesign1.Id, ticketDesign1);
            return ResponseDto<NoContent>.Success(new NoContent(), 204);
        }
    }
}
