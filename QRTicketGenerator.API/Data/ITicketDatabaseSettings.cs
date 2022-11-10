namespace QRTicketGenerator.API.Data
{
    public interface ITicketDatabaseSettings
    {
        string BooksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
