using QRTicketGenerator.API.Models;

namespace QRTicketGenerator.API.Data
{

    public class TicketDatabaseSettings:ITicketDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
