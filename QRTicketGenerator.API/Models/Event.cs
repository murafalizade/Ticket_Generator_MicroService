using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace QRTicketGenerator.API.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}
