﻿using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
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
        Task<Post> GetPostByIdAsync(int postId);
        Task<IEnumerable<Post>> GetRelatedPostsAsync(int clubId, int excludePostId, int count);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);

    }
}
