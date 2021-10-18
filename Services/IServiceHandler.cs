﻿using System.Collections.Generic;
using System.Threading.Tasks;
using zmgTestBack.Models;

namespace zmgTestBack.Services
{
    /// <summary>
    /// An interface to perform the basic operations over the blog posts and comments.
    /// </summary>
    public interface IServiceHandler
    {
        Task<Post> GetPostById(decimal postId, decimal userId);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetPostsByUser(decimal userId);
        Task<List<Post>> GetPendingPosts();
        Task CreatePost(PostRequest post);
        Task UpdatePost(PostRequest post, decimal userId);
        Task DeletePost(decimal postId);
        Task CreateComment(CommentRequest comment);
        Task DeleteComment(decimal commentId);

        Task RejectPost(decimal postId);
        Task ApprovePost(decimal postId);

        Task LikePost(decimal postId);
        Task DislikePost(decimal postId);

        Task LikeComment(decimal commentId);
        Task DislikeComment(decimal commentId);
    }
}
