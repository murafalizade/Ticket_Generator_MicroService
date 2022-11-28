using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QRTicketGenerator.API.Models
{
    public class TicketDesign
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Userid { get; set; }
        public string DesignFilePath { get; set; } = "uploads/TicketDesign.pdf";
        public float QrCodeX { get; set; } = 340;
        public float QrCodeY { get; set; } = 30;
        public int ValueCount { get; set; } = 0;
        public float PositionX1 { get; set; } = 0;
        public float PositionY1 { get; set; } = 0;
        public int fontSize1 { get; set; } = 8;
        public float PositionX2 { get; set; } = 0;
        public float PositionY2 { get; set; } = 0;
        public int fontSize2 { get; set; } = 8;
        public float PositionX3 { get; set; } = 0;
        public float PositionY3 { get; set; } = 0;
        public int fontSize3 { get; set; } = 8;
        public float PositionX4 { get; set; } = 0;
        public float PositionY4 { get; set; } = 0;
        public int fontSize4 { get; set; } = 8;
    }
}
