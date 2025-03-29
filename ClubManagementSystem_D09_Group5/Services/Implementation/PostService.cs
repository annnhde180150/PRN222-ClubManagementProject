using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IClubMemberService _clubMemberService;
        private readonly ICommentService _commentService;
        private readonly IImageHelperService _imageHelperService;
        public PostService(IPostRepository postRepository, IClubMemberService clubMemberService, ICommentService commentService , IImageHelperService imageHelperService)
        {
            _postRepository = postRepository;
            _clubMemberService = clubMemberService;
            _commentService = commentService;
            _imageHelperService = imageHelperService;
        }

        public async Task<IEnumerable<Post>> GetAllPostsByClubIdAsync(int clubId)
        {
            return await _postRepository.GetAllPostByClubIdAsync(clubId);
        }
        public async Task<Post> CreatePostAsync(Post model, IFormFile? imageFile, int userId, int clubId)
        {
            bool isMember = await _clubMemberService.IsUserInClubAsync(userId, clubId);
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
            var relatedPosts = await _postRepository.GetRelatedPostsAsync(post.ClubMember.ClubId, post.PostId, 3);

            var comments = await _commentService.GetCommentsByPostIdAsync(postId);

            var viewModel = new PostDetailsDto
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                ImageBase64 = _imageHelperService.ConvertToBase64(post.Image, "png"),
                CreatedAt = post.CreatedAt,
                Status = post.Status,
               
                Club = new ClubDto
                {
                    ClubId = post.ClubMember.ClubId,
                    ClubName = post.ClubMember.Club.ClubName
                },
                Comments = post.Comments.Select(c => new CommentDto
                {
                    CommentId = c.CommentId,
                    PostId = c.PostId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    User = new UserDto
                    {
                        UserId = c.User.UserId,  
                        Username = c.User.Username,
                        Email = c.User.Email,
                        ProfilePictureBase64 = _imageHelperService.ConvertToBase64(c.User.ProfilePicture, "png"),
                    }
                }).ToList()

            };

            if (viewModel == null)
            {
                return null;
            }

            return viewModel;

        }

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.UpdatePostAsync(post);
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _postRepository.GetPostByIdAsync(postId);
        }

        public async Task DeletePostAsync(int id)
        {
            await _postRepository.DeletePostAsync(id);
        }

    }
}
