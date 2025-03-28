﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;
using Services.Interface;
using BussinessObjects.Models.Dtos;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Repositories.Interface;
using Services.Implementation;
using Microsoft.Extensions.Hosting;

namespace ClubManagementSystem.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IClubRequestService _clubRequestService;
        private readonly IClubService _clubService;
        private readonly IPostService _postService;
        private readonly FptclubsContext _context;

        public ClubsController(FptclubsContext context , IClubRequestService clubRequestService, IAccountService accountService, IClubService clubService, IPostService postService)
        {
            _context = context;
            _clubRequestService = clubRequestService;
            _accountService = accountService;
            _clubService = clubService;
            _postService = postService;
        }

        // GET: Clubs
        [Authorize("SystemAdmin")]
        public async Task<IActionResult> Index()
        {
            var clubRequest = _clubRequestService.GetAllClubRequestPendingAsync();
            return View(clubRequest);
        }



        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id, int? pageNumber)
        {
            int postSize = 5;
            int currentPage = pageNumber ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _clubService.GetClubDetailsAsync(id.Value, currentPage, postSize);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }


        //[Authorize]
        //// GET: Clubs/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile logoPicture,string clubName, string description,IFormFile coverPicture)
        {
            if ((logoPicture != null && logoPicture.Length > 0) || (coverPicture != null && coverPicture.Length > 0))
            {
                // Convert image to byte array
                byte[] logoPictureBytes;
                byte[] coverPictureBytes;
                using (var logomemoryStream = new MemoryStream())
                {
                    await logoPicture.CopyToAsync(logomemoryStream);
                    logoPictureBytes = logomemoryStream.ToArray();                   
                }
                using (var covermemoryStream = new MemoryStream())
                {
                    await coverPicture.CopyToAsync(covermemoryStream);
                    coverPictureBytes = covermemoryStream.ToArray();
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var clubRequest = new ClubRequest
                    {
                        ClubName = clubName,
                        Description = description,
                        Logo = logoPictureBytes,
                        Cover = coverPictureBytes,
                        CreatedAt = DateTime.Now,
                        UserId = int.Parse(userId),
                        Status = "Pending"
                    };
                    await _clubRequestService.AddClubRequestAsync(clubRequest);
                    TempData["SuccessMessage"] = "Club created successfully!";
                    return RedirectToAction("Index", "Home");
                }                                  
            }
            TempData["ErrorMessage"] = "Please fill logo picture and cover picture!";
            return View();
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClubId,ClubName,Description,CreatedAt")] Club club)
        {
            if (id != club.ClubId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubId))
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
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club != null)
            {
                _context.Clubs.Remove(club);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.ClubId == id);
        }
    }
}
