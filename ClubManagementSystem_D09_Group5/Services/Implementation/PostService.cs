using BussinessObjects.Models;
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
        public PostService(IPostRepository postRepository, IClubMemberRepository clubMemberRepository)
        {
            _postRepository = postRepository;
            _clubMemberRepository = clubMemberRepository;
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
    }
}
