using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventService
    {
        public Task<Event> AddEventAsync(Event Event);
        public Task<bool> UpdateEventAsync(Event Event);
        public Task<IEnumerable<Event>> GetEventsAsync();
        public Task<Event> GetEventAsync(int EventId);
    }
}
