using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRTicketGenerator.Shared.Messages
{
    public class UserCreateCommand
    {
        public string Id { get; set; }
        public int CoinCount { get; set; }
        public bool isPremium { get; set; }
    }
}
