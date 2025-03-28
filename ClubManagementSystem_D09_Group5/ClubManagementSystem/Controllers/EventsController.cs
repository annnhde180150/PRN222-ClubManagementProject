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

namespace ClubManagementSystem.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        //delete context when finish CRUD
        private readonly FptclubsContext _context;
        private readonly IEventService _eventService;
        private readonly Week _week;

        public EventsController(FptclubsContext context, IEventService eventService, Week week)
        {
            _context = context;
            _eventService = eventService;
            _week = week;
        }


        // GET: Events
        public async Task<IActionResult> Index(int? week, int? clubID)
        {
            if(clubID == null) return RedirectToAction("Index","JoinRequests");

            var currentWeek = week != null ? _week.GetWeekRange(DateTime.Now.Year, week.Value) : _week.GetWeekRange(DateTime.Now);
            var events = await _eventService.GetEventsAsync(currentWeek.StartOfWeek, currentWeek.EndOfWeek, clubID.Value);
            var weekDays = _week.GetWeekDays(currentWeek.StartOfWeek);
            var dropdownList = _week.GetWeekDropDown(DateTime.Now.Year);
            var selectList = new SelectList(dropdownList, "Value", "Text");

            ViewBag.WeekDays = weekDays;
            ViewBag.WeekDropdown = selectList;
            ViewBag.ClubID = clubID.Value;

            return View(events);
        }

        ////GET: Events/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var @event = await _context.Events
        //        .Include(@ => @.CreatedByNavigation)
        //        .FirstOrDefaultAsync(m => m.EventId == id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(@event);
        //}

        ////GET: Events/Create
        //public IActionResult Create()
        //{
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId");
        //    return View();
        //}

        //// POST: Events/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("EventId,CreatedBy,EventTitle,EventDescription,EventDate,CreatedAt")] Event @event)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(@event);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId", @event.CreatedBy);
        //    return View(@event);
        //}

        //// GET: Events/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var @event = await _context.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId", @event.CreatedBy);
        //    return View(@event);
        //}

        //// POST: Events/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("EventId,CreatedBy,EventTitle,EventDescription,EventDate,CreatedAt")] Event @event)
        //{
        //    if (id != @event.EventId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(@event);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EventExists(@event.EventId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId", @event.CreatedBy);
        //    return View(@event);
        //}

        //// GET: Events/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var @event = await _context.Events
        //        .Include(@ => @.CreatedByNavigation)
        //        .FirstOrDefaultAsync(m => m.EventId == id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(@event);
        //}

        //// POST: Events/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var @event = await _context.Events.FindAsync(id);
        //    if (@event != null)
        //    {
        //        _context.Events.Remove(@event);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EventExists(int id)
        //{
        //    return _context.Events.Any(e => e.EventId == id);
        //}
    }
}
