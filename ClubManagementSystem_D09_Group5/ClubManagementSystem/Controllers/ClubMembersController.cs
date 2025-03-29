using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Services.Interface;
using BussinessObjects.Models.Dtos;

namespace ClubManagementSystem.Controllers
{
    public class ClubMembersController : Controller
    {
        private readonly FptclubsContext _context;
        private readonly IClubMemberService _clubMemberService;
        private readonly IImageHelperService _imageService;

        public ClubMembersController(FptclubsContext context, IClubMemberService clubMemberService, IImageHelperService imageHelperService)
        {
            _context = context;
            _clubMemberService = clubMemberService;
            _imageService = imageHelperService;
        }

        [Authorize(Roles ="Admin")]
        // GET: ClubMembers
        public async Task<IActionResult> Index(int clubId)
        {
            if (clubId == 0)
            {
                NotFound();
            }
            var clubMembers = await _clubMemberService.GetClubMembersByClubIdAsync(clubId);

            var clubmemberView = clubMembers.Select(member =>
            {
                string profilePictureString = "";
                if (member.User.ProfilePicture != null)
                {
                    profilePictureString = _imageService.ConvertToBase64(member.User.ProfilePicture, "png");
                }

                return new ClubMeberIndexDto
                {
                    MembershipId = member.MembershipId,
                    Username = member.User.Username,
                    ProfilePictureBase64 = profilePictureString,
                    Role = member.Role.RoleName,
                    JoinedAt = member.JoinedAt
                };
            });
            return View(clubmemberView.ToList());
        }

        public async Task<IActionResult> AssignRole(int membershipId, string role)
        {
            if (membershipId == 0)
            {
                NotFound();
            }
            var clubMember = await _clubMemberService.GetClubMemberByIdAsync(membershipId);
            int roleCheck = 0;
            //Check role Admin
            if (role.Equals("Admin"))
            {
                roleCheck = 1;
            }
            //Check role Morderator
            if (role.Equals("Morderator"))
            {
                roleCheck = 2;
            }
            //Check role Member
            if (role.Equals("Member"))
            {
                roleCheck = 3;
            }
            clubMember.RoleId = roleCheck;
            var (success ,message) = await _clubMemberService.UpdateClubMemberAsync(clubMember);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction("Index", new { clubId = clubMember.ClubId });
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
