﻿using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IAccountService _accountService;
        private readonly IImageHelperService _imageHelperService;

        public ClubService(IClubRepository clubRepository, IAccountService accountService, IImageHelperService imageHelperService)
        {
            _clubRepository = clubRepository;
            _accountService = accountService;
            _imageHelperService = imageHelperService;
        }

        public async Task<ClubDetailsViewDto> GetClubDetailsAsync(int clubId, int postNumber, int postSize)
        {
            var club = await _clubRepository.GetClubByIdWithMembersPostsAsync(clubId);

            if (club == null) return null;

            var clubMemberDtos = club.ClubMembers
                .Select(member => new ClubMemberDto
                {
                    UserId = member.UserId,
                    Username = member.User.Username,
                    ProfilePictureBase64 = _imageHelperService.ConvertToBase64(member.User.ProfilePicture, "png")
                })
                .ToList();

            var postsQuery = club.ClubMembers
        .SelectMany(member => member.Posts)
        .OrderByDescending(post => post.CreatedAt); // Order by newest posts

            var totalPosts = postsQuery.Count();
            var posts = postsQuery.Skip((postNumber - 1) * postSize).Take(postSize).ToList();

            var postDtos = posts.Select(post => new PostDto
            {
                PostId = post.PostId,
                Content = post.Content,
                ImageBase64 = _imageHelperService.ConvertToBase64(post.Image, "png"),
                CreatedAt = post.CreatedAt,
                Status = post.Status,
                CreatedBy = post.CreatedBy,
                CreatedByUsername = post.CreatedByNavigation.User.Username
            }).ToList();

            return new ClubDetailsViewDto
            {
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                Logo = club.Logo,
                Cover = club.Cover,
                Description = club.Description,
                ClubMembers = clubMemberDtos,
                Posts = postDtos,

                TotalPosts = totalPosts,
                PostNumber = postNumber,
                PostSize = postSize
            };
        }
        public async Task AddClubAsync(Club club)
        {
            await _clubRepository.AddClubAsync(club);
        }

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
            return await _clubRepository.GetAllClubAsync();
        }

        public async Task<(bool success, string message)> UpdateClubAsync(ClubEditDto clubEditDto)
        {
            var clubs = await _clubRepository.GetAllClubAsync();
            var clubCheck = clubs.FirstOrDefault(m => m.ClubName.Equals(clubEditDto.ClubName) && m.ClubId != clubEditDto.ClubId);
            if (clubCheck != null)
            {
                return (false, "This club name already exist!");
            }
            var club = clubs.FirstOrDefault(m => m.ClubId == clubEditDto.ClubId);
            if(!String.IsNullOrEmpty(clubEditDto.ClubName) && !String.IsNullOrEmpty(clubEditDto.Description))
            {
                club.ClubName = clubEditDto.ClubName;
                club.Description = clubEditDto.Description;
            }
            if (clubEditDto.Logo != null)
            {
                club.Logo = clubEditDto.Logo;
            }
            if (clubEditDto.Cover != null)
            {
                club.Cover = clubEditDto.Cover;
            }
            await _clubRepository.UpdateClubAsync(club);
            return (true, "Club update successfully!");
        }

        public async Task<Club> GetClubByClubIdAsync(int clubId)
        {
            return await _clubRepository.GetClubByClubIdAsync(clubId);
        }
    }
}