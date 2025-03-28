using BussinessObjects.Models;
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

            var postDtos = posts.Select(post => new PostDetailsDto
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                ImageBase64 = _imageHelperService.ConvertToBase64(post.Image, "png"),
                CreatedAt = post.CreatedAt,
                Status = post.Status,
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

        
    }
}