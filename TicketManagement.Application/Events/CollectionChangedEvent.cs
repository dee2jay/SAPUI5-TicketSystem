using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Interfaces;

namespace TicketManagement.Application.Events
{
    public class CollectionChangedEvent<T> : IDomainEvent
    {
        public int TicketId { get; }
        public string CollectionName { get; }
        public string ChangeType { get; } // "Added" / "Removed"
        public T? Item { get; }
        public string TriggeredBy { get; }

        public CollectionChangedEvent(int ticketId, string collectionName, string changeType, T? item, string triggeredBy)
        {
            TicketId = ticketId;
            CollectionName = collectionName;
            ChangeType = changeType;
            Item = item;
            TriggeredBy = triggeredBy;
        }       
    }
}
