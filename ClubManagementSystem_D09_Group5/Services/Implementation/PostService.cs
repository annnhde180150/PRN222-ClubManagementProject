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
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Post> CreatePostAsync(Post model, IFormFile? imageFile, int userId)
        {
            var post = new Post
            {
                CreatedBy = userId,
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
