using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [AllowNull]
        public string DelegateName { get; set; }
        public int EventId { get; set; }
        public bool IsConfirmed { get; set; } = false;

        public static implicit operator Task<object>(Ticket v)
        {
            throw new NotImplementedException();
        }
    }
}
