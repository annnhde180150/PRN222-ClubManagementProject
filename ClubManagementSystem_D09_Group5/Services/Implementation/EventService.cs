using BussinessObjects.Models;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService (IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Event> AddEventAsync(Event Event)
        {
            return await _eventRepository.AddEventAsync(Event);
        }

        public async Task<Event> GetEventAsync(int EventId)
        {
            return await _eventRepository.GetEventAsync(EventId);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _eventRepository.GetEventsAsync();
        }

        public async Task<bool> UpdateEventAsync(Event Event)
        {
            return await _eventRepository.UpdateEventAsync(Event);
        }
    }
}
