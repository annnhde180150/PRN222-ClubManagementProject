using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;
using Services.Interface;
using ClubManagementSystem.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Services.Implementation;

namespace ClubManagementSystem.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        //delete context when finish CRUD
        private readonly FptclubsContext _context;
        private readonly IEventService _eventService;
        private readonly IClubMemberService _clubMemberService;
        private readonly Week _week;

        public EventsController(FptclubsContext context, IEventService eventService, Week week, IClubMemberService clubMemberService)
        {
            _context = context;
            _eventService = eventService;
            _week = week;
            _clubMemberService = clubMemberService;
        }


        // GET: Events
        //event :_________| Monday | Tue | Wed | Thu | Fri | Sat | Sun
        //       Morning  |
        //       Noon     |
        //       Afternoon|
        //       Evening  |
        public async Task<IActionResult> Index(int? week, int? clubID, string? error)
        {
            if(clubID == null) return RedirectToAction("Index","Clubs");

            var currentWeek = week != null ? _week.GetWeekRange(DateTime.Now.Year, week.Value) : _week.GetWeekRange(DateTime.Now);
            var eventsList = await _eventService.GetEventsAsync(currentWeek.StartOfWeek, currentWeek.EndOfWeek, clubID.Value);
            var weekDays = _week.GetWeekDays(currentWeek.StartOfWeek);
            var dropdownList = _week.GetWeekDropDown(DateTime.Now.Year);
            var selectList = new SelectList(dropdownList, "Value", "Text");
            var days = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .OrderBy(d => (d == DayOfWeek.Sunday) ? 7 : (int)d)
                .Select(d => d.ToString())
                .ToList();
            var events = new List<Dictionary<String, Event>>()
            {
                new Dictionary<String, Event>(),
                new Dictionary<String, Event>(),
                new Dictionary<String, Event>(),
                new Dictionary<String, Event>()
            };

            foreach (var ev in eventsList)
            {
                var day = ev.EventDate.DayOfWeek.ToString();
                int slot;

                if (ev.EventDate.TimeOfDay < TimeSpan.FromHours(11)) slot = 0;
                else if (ev.EventDate.TimeOfDay < TimeSpan.FromHours(14)) slot = 1;
                else if (ev.EventDate.TimeOfDay < TimeSpan.FromHours(17)) slot = 2;
                else slot = 3;

                events[slot][day] = ev;
            }

            ViewBag.Error = error ?? null;
            ViewBag.DaysOfWeek = days;
            ViewBag.WeekDays = weekDays;
            ViewBag.WeekDropdown = selectList;
            ViewBag.ClubID = clubID.Value;

            return View(events);
        }

        //GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentEvent = await _eventService.GetEventAsync(id.Value);
            return View(currentEvent);
        }

        //GET: Events/Create
        public IActionResult Create(string? error, int id)
        {
            ViewBag.ClubID = id;
            if (error != null) ViewBag.Error = error;
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int clubID, [Bind("EventTitle,EventDescription,EventDate")] Event @event)
        {
            @event.CreatedAt = DateTime.Now;
            //wrong created by
            var clubMem = await _clubMemberService.GetClubMemberAsync(clubID, Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            @event.CreatedBy = clubMem.MembershipId;
            @event.Status = "Not Yet";
            var isOccupied = await _eventService.isOccupied(@event.EventDate, clubID);

            if (!isOccupied)
            {
                await _eventService.AddEventAsync(@event);
            }

            return RedirectToAction("Index", "Events", new { clubID = clubID , error = "Cannot Create Due to Occupied Event slot!"});
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var @event = await _eventService.GetEventAsync(id.Value);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventTitle,EventDescription,EventDate")] Event @event)
        {
            
            var currentEvent = await _eventService.GetEventAsync(id);
            currentEvent.EventDate = @event.EventDate;
            currentEvent.EventTitle = @event.EventTitle;
            currentEvent.EventDescription = @event.EventDescription;
            await _eventService.UpdateEventAsync(currentEvent);
            return RedirectToAction("Index", "Events", new { clubID = currentEvent.CreatedByNavigation.ClubId });
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var currentEvent = await _eventService.GetEventAsync(id.Value);
            return View(currentEvent);
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _eventService.GetEventAsync(id);
            if (!(await _eventService.IsDependedOn(id)) && @event.Status.Equals("Not Yet"))
            {
                @event.Status = "Cancelled";
                await _eventService.UpdateEventAsync(@event);
            }
            return RedirectToAction("Index","Events", new { clubID = @event.CreatedByNavigation.ClubId });
        }
    }
}
