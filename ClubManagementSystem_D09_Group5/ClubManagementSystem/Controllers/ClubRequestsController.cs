using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;

namespace ClubManagementSystem.Controllers
{
    public class ClubRequestsController : Controller
    {
        private readonly FptclubsContext _context;

        public ClubRequestsController(FptclubsContext context)
        {
            _context = context;
        }

        // GET: ClubRequests
        public async Task<IActionResult> Index()
        {
            var fptclubsContext = _context.ClubRequests.Include(c => c.User);
            return View(await fptclubsContext.ToListAsync());
        }

        // GET: ClubRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubRequest = await _context.ClubRequests
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (clubRequest == null)
            {
                return NotFound();
            }

            return View(clubRequest);
        }

        // GET: ClubRequests/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: ClubRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,UserId,ClubName,Description,Logo,Cover,Status,CreatedAt")] ClubRequest clubRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", clubRequest.UserId);
            return View(clubRequest);
        }

        // GET: ClubRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubRequest = await _context.ClubRequests.FindAsync(id);
            if (clubRequest == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", clubRequest.UserId);
            return View(clubRequest);
        }

        // POST: ClubRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,UserId,ClubName,Description,Logo,Cover,Status,CreatedAt")] ClubRequest clubRequest)
        {
            if (id != clubRequest.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubRequestExists(clubRequest.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", clubRequest.UserId);
            return View(clubRequest);
        }

        // GET: ClubRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubRequest = await _context.ClubRequests
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (clubRequest == null)
            {
                return NotFound();
            }

            return View(clubRequest);
        }

        // POST: ClubRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clubRequest = await _context.ClubRequests.FindAsync(id);
            if (clubRequest != null)
            {
                _context.ClubRequests.Remove(clubRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubRequestExists(int id)
        {
            return _context.ClubRequests.Any(e => e.RequestId == id);
        }
    }
}
