using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;
using Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ClubManagementSystem.Controllers
{
    [Authorize]
    public class ClubTasksController : Controller
    {
        //delet after
        private readonly FptclubsContext _context;
        private readonly IClubTaskService _taskService;

        public ClubTasksController(FptclubsContext context, IClubTaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        //GET: ClubTasks
        public async Task<IActionResult> Index()
        {
            int userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tasks = await _taskService.GetPersonalClubTasksAsync(userID);
            return View(tasks);
        }

        // GET: ClubTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var task = await _taskService.GetClubTask(id.Value);
            return View(task);
        }

        //// GET: ClubTasks/Create
        //public IActionResult Create()
        //{
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId");
        //    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventTitle");
        //    return View();
        //}

        //// POST: ClubTasks/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TaskId,EventId,TaskDescription,Status,DueDate,CreatedAt,CreatedBy")] ClubTask clubTask)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(clubTask);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId", clubTask.CreatedBy);
        //    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventTitle", clubTask.EventId);
        //    return View(clubTask);
        //}

        //// GET: ClubTasks/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var clubTask = await _context.Tasks.FindAsync(id);
        //    if (clubTask == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId", clubTask.CreatedBy);
        //    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventTitle", clubTask.EventId);
        //    return View(clubTask);
        //}

        //// POST: ClubTasks/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("TaskId,EventId,TaskDescription,Status,DueDate,CreatedAt,CreatedBy")] ClubTask clubTask)
        //{
        //    if (id != clubTask.TaskId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(clubTask);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClubTaskExists(clubTask.TaskId))
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
        //    ViewData["CreatedBy"] = new SelectList(_context.ClubMembers, "MembershipId", "MembershipId", clubTask.CreatedBy);
        //    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventTitle", clubTask.EventId);
        //    return View(clubTask);
        //}

        //// GET: ClubTasks/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var clubTask = await _context.Tasks
        //        .Include(c => c.CreatedByNavigation)
        //        .Include(c => c.Event)
        //        .FirstOrDefaultAsync(m => m.TaskId == id);
        //    if (clubTask == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(clubTask);
        //}

        //// POST: ClubTasks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var clubTask = await _context.Tasks.FindAsync(id);
        //    if (clubTask != null)
        //    {
        //        _context.Tasks.Remove(clubTask);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ClubTaskExists(int id)
        //{
        //    return _context.Tasks.Any(e => e.TaskId == id);
        //}
    }
}
