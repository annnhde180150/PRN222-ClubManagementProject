using BussinessObjects.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(Post model, IFormFile? imageFile, int userId, int clubId);
        Task<IEnumerable<Post>> GetAllPostsByClubIdAsync(int clubId);

    }
}
