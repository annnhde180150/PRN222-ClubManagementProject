using BussinessObjects.Models;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class PostRepository : IPostRepository
    {
        private readonly FptclubsContext _context;
        public PostRepository(FptclubsContext context)
        {
            _context = context;
        }
        public async Task<Post> AddAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
    }
}
