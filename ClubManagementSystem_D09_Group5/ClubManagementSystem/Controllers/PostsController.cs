﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;
using System.Security.Claims;
using Services.Interface;
using BussinessObjects.Models.Dtos;
using Services.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace ClubManagementSystem.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IClubMemberService _clubMemberService;
        public PostsController(IPostService postService, IClubMemberService clubMemberService)
        {
            _postService = postService;
            _clubMemberService = clubMemberService;
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (id == null)
            {
                return NotFound();
            }

            var postDetails = await _postService.GetPostDetailsByIdAsync(id.Value, userId);

            if (postDetails == null)
            {
                return NotFound();
            }

            ViewBag.IsMember = await _clubMemberService.IsClubMember(id.Value, userId);
            return View(postDetails);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post model, IFormFile? ImageFile, int clubId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            try
            {
                var post = await _postService.CreatePostAsync(model, ImageFile, userId, clubId);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Details", "Clubs", new { id = clubId });
            }

            return RedirectToAction("Details", "Clubs", new { id = clubId });
        }



        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostUpdateDto postDto, IFormFile? ImageFile)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            var existingPost = await _postService.GetPostByIdAsync(postDto.PostId);
            if (existingPost == null || existingPost.CreatedBy != userId)
            {
                return Forbid();
            }

            existingPost.Title = postDto.Title;
            existingPost.Content = postDto.Content;

            if (ImageFile != null)
            {
                using var ms = new MemoryStream();
                await ImageFile.CopyToAsync(ms);
                existingPost.Image = ms.ToArray();
            }

            try
            {
                await _postService.UpdatePostAsync(existingPost);
                return RedirectToAction("Details", "Posts", new { id = existingPost.PostId });
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int postId, int clubId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null || post.CreatedBy != userId)
            {
                return Forbid(); // Ensure only the owner can delete
            }

            await _postService.DeletePostAsync(postId);

            // Redirect back to the Club Details page
            return RedirectToAction("Details", "Clubs", new { id = clubId });
        }

       

      
    }
}
