using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

        public async Task<IEnumerable<Event>> GetEventsAsync(int clubID)
        {
            return (await _eventRepository.GetEventsAsync()).Where(e => e.CreatedByNavigation.ClubId == clubID);
        }

        public async Task<bool> UpdateEventAsync(Event Event)
        {
            return await _eventRepository.UpdateEventAsync(Event);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(DateTime start, DateTime end, int clubID)
        {
            var events = await _eventRepository.GetEventsAsync();
            return events
                .Where(e => e.EventDate >= start && e.EventDate <= end)
                .Where(e => e.CreatedByNavigation.ClubId == clubID)
                .Where(e => e.Status!="Cancelled");
        }

        public async Task<bool> IsDependedOn(int EventID)
        {
            var currentEvent = await _eventRepository.GetEventAsync(EventID);
            if (currentEvent.Tasks == null) return false;
            foreach (var item in currentEvent.Tasks)
            {
                if (item.TaskAssignments.Count > 0)
                    return true;
            }
            return false;
        }

        public async Task<bool> isOccupied(DateTime EventDate,int clubID)
        {
            var slotTime = TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59)).Add(TimeSpan.FromSeconds(59));
            if (EventDate.TimeOfDay < TimeSpan.FromHours(11)) slotTime = TimeSpan.FromHours(11);
            else if (EventDate.TimeOfDay < TimeSpan.FromHours(14)) slotTime = TimeSpan.FromHours(14);
            else if (EventDate.TimeOfDay < TimeSpan.FromHours(17)) slotTime = TimeSpan.FromHours(17);

            var @event = (await GetEventsAsync(clubID))
                .Where(e => e.Status != "Cancelled")
                .Where(e => e.EventDate.Date == EventDate.Date && EventDate.TimeOfDay < slotTime);
            return @event.Any();
        }

        public async Task<Event> GetIncomingEvent()
        {
            return (await _eventRepository.GetEventsAsync())
                .Where(e => e.EventDate > DateTime.Now)
                .OrderBy(e => e.EventDate)
                .FirstOrDefault();
        }
    }
}
