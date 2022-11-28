namespace QRTicketGenerator.API.Dtos
{
    public class MailSenderDto
    {
        // Swagger required to property
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToCC { get; set; }
        public string ToBCC { get; set; }
    }
}
