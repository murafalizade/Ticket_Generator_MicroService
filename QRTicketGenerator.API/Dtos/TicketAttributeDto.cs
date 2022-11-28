using System.Drawing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QRTicketGenerator.API.Dtos
{
    public class TicketAttributeDto
    {
        public string DelegateName { get; set; }
        public string EventId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketDesignId { get; set; }
        public string Value1 { get; set; } = "";
        public string Value2 { get; set; } = "";
        public string Value3 { get; set; } = "";
        public string Value4 { get; set; } = "";
    }
}
