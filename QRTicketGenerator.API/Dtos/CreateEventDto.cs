using System;

namespace QRTicketGenerator.API.Dtos
{
    public class CreateEventDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
