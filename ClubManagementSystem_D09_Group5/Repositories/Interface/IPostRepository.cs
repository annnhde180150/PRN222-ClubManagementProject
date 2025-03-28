using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IPostRepository
    {
        Task<Post> AddAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostByClubIdAsync(int clubId);
    }
}
