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
    public class ClubMembersController : Controller
    {
        private readonly FptclubsContext _context;

        public ClubMembersController(FptclubsContext context)
        {
            _context = context;
        }

        // GET: ClubMembers
        public async Task<IActionResult> Index()
        {
            var fptclubsContext = _context.ClubMembers.Include(c => c.Club).Include(c => c.Role).Include(c => c.User);
            return View(await fptclubsContext.ToListAsync());
        }

        // GET: ClubMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMember = await _context.ClubMembers
                .Include(c => c.Club)
                .Include(c => c.Role)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (clubMember == null)
            {
                return NotFound();
            }

            return View(clubMember);
        }

        // GET: ClubMembers/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: ClubMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,ClubId,UserId,RoleId,JoinedAt")] ClubMember clubMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", clubMember.ClubId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", clubMember.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", clubMember.UserId);
            return View(clubMember);
        }

        // GET: ClubMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMember = await _context.ClubMembers.FindAsync(id);
            if (clubMember == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", clubMember.ClubId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", clubMember.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", clubMember.UserId);
            return View(clubMember);
        }

        // POST: ClubMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipId,ClubId,UserId,RoleId,JoinedAt")] ClubMember clubMember)
        {
            if (id != clubMember.MembershipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubMemberExists(clubMember.MembershipId))
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", clubMember.ClubId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", clubMember.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", clubMember.UserId);
            return View(clubMember);
        }

        // GET: ClubMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMember = await _context.ClubMembers
                .Include(c => c.Club)
                .Include(c => c.Role)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (clubMember == null)
            {
                return NotFound();
            }

            return View(clubMember);
        }

        // POST: ClubMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clubMember = await _context.ClubMembers.FindAsync(id);
            if (clubMember != null)
            {
                _context.ClubMembers.Remove(clubMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubMemberExists(int id)
        {
            return _context.ClubMembers.Any(e => e.MembershipId == id);
        }
    }
}
