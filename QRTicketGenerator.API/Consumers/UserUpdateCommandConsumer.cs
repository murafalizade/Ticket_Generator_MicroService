using MassTransit;
using MongoDB.Driver;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.Shared.Messages;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Consumers
{
    public class UserUpdateCommandConsumer : IConsumer<UserCreateCommand>
    {
        private readonly IMongoCollection<User> _users;

        public UserUpdateCommandConsumer(ITicketDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }
        public async Task Consume(ConsumeContext<UserCreateCommand> context)
        {
            User user = await _users.Find(s => s.Id == context.Message.Id).FirstOrDefaultAsync();
            await _users.ReplaceOneAsync(x => x.Id == context.Message.Id, new User()
            {
                Id = context.Message.Id,
                CoinCount = user.CoinCount + context.Message.CoinCount,
                isPremium = context.Message.isPremium
            });
        }
    }
}
