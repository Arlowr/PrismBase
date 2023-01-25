using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismBase.Modules.Details.Models
{
    public class Client : Person
    {
        public class ClientUpdatedEvent : PubSubEvent<Client> { }
        public class ClientDBUpdatedEvent : PubSubEvent { }
        public int ClientId { get; set; }
        public List<string> Tags { get; set; }
        public List<Note> Notes { get; set; }
    }
}
