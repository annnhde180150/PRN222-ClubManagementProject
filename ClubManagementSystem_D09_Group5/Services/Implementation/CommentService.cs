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
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAccountService _accountService;
        private readonly IImageHelperService _imageHelperService;

        public CommentService(ICommentRepository commentRepository, IAccountService accountService, IImageHelperService imageHelperService)
        {
            _commentRepository = commentRepository;
            _accountService = accountService;
            _imageHelperService = imageHelperService;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _commentRepository.GetAllCommentsAsync();
        }

        public async Task<Comment?> GetCommentAsync(int id)
        {
            return await _commentRepository.GetCommentAsync(id);
        }
       
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            return await _commentRepository.AddCommentAsync(comment);
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            return await _commentRepository.UpdateCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _commentRepository.DeleteCommentAsync(id);
        }


        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);

            var commentDtos = new List<CommentDto>();

            foreach (var comment in comments)
            {
                var user = await _accountService.FindUserAsync(comment.UserId);
                commentDtos.Add(new CommentDto
                {
                    CommentId = comment.CommentId,
                    PostId = comment.PostId,
                    CommentText = comment.CommentText,
                    CreatedAt = comment.CreatedAt,

                    User = new UserDto() 
                    {
                        UserId = comment.User.UserId,
                        Username = comment.User.Username,
                        Email = comment.User.Email,
                        ProfilePictureBase64 = _imageHelperService.ConvertToBase64(comment.User.ProfilePicture, "png")
                    }
                });
            }

            return commentDtos;
        }
    }

}
