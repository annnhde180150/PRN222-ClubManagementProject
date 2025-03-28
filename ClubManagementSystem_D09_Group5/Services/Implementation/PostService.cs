using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IClubMemberRepository _clubMemberRepository;
        private readonly IImageHelperService _imageHelperService;
        public PostService(IPostRepository postRepository, IClubMemberRepository clubMemberRepository, IImageHelperService imageHelperService)
        {
            _postRepository = postRepository;
            _clubMemberRepository = clubMemberRepository;
            _imageHelperService = imageHelperService;
        }

        public async Task<IEnumerable<Post>> GetAllPostsByClubIdAsync(int clubId)
        {
            return await _postRepository.GetAllPostByClubIdAsync(clubId);
        }
        public async Task<Post> CreatePostAsync(Post model, IFormFile? imageFile, int userId, int clubId)
        {
            bool isMember = await _clubMemberRepository.IsUserInClubAsync(userId, clubId);
            if (!isMember)
            {
                throw new UnauthorizedAccessException("User is not a member of the club.");
            }

            var post = new Post
            {
                CreatedBy = userId,
                Title = model.Title,
                Content = model.Content,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await imageFile.CopyToAsync(ms);
                post.Image = ms.ToArray();
            }

            return await _postRepository.AddAsync(post);
        }

        public async Task<PostDetailsDto?> GetPostDetailsByIdAsync(int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null)
            {
                return null; 
            }
            var relatedPosts = await _postRepository.GetRelatedPostsAsync(post.CreatedByNavigation.Club.ClubId, post.PostId, 3);

            var viewModel = new PostDetailsDto
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                ImageBase64 = _imageHelperService.ConvertToBase64(post.Image, "png"),
                CreatedAt = post.CreatedAt,
                Status = post.Status,
                CreatedBy = post.CreatedBy,
                CreatedByUsername = post.CreatedByNavigation.User.Username,
                ClubName = post.CreatedByNavigation.Club.ClubName,
                RelatedPosts = relatedPosts.Select(p => new RelatedPostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    ImageBase64 = _imageHelperService.ConvertToBase64(p.Image, "png"),
                }).ToList()
            };

            if (viewModel == null)
            {
                return null;
            }

            return viewModel;

        }
    }
}
