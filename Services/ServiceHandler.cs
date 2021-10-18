using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zmgTestBack.Models;

namespace zmgTestBack.Services
{
    public class ServiceHandler : IServiceHandler
    {
        private ZmgTestDbContext _dbContext;

        public ServiceHandler(ZmgTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ApprovePost(decimal postId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId && x.Status == (int)StatusEnums.PendingApproval);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be Approved", postId));
            postToUpdate.ModificationDate = DateTime.UtcNow;
            postToUpdate.Status = (int)StatusEnums.Published;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateComment(CommentRequest comment)
        {
            int repeatedComments = _dbContext.Comments
                .Where(x => x.Status == (int)StatusEnums.Published && x.PostId == comment.PostId && x.CreationUser.UserId == comment.CreationUserId && x.Contents == comment.Contents).Count();
            Post post = _dbContext.Posts
                .SingleOrDefault(x => x.PostId == comment.PostId);
            User user = _dbContext.Users
                .SingleOrDefault(x => x.UserId == comment.CreationUserId);
            if (repeatedComments > 2)
                throw new DuplicateWaitObjectException("This comment couldn't be posted because you have already posted the same comment several times before.");
            if(post == null)
                throw new ArgumentNullException("The post where you are trying to comment does not exist");
            Comment commentToSave = new Comment()
            {
                Contents = comment.Contents,
                CreationUserId = comment.CreationUserId,
                CreationDate = DateTime.UtcNow,
                Likes = 0,
                Dislikes = 0,
                PostId = comment.PostId,
                IsReview = post.Status == (int)StatusEnums.PendingApproval? true : false,
                Status = (int)StatusEnums.Published,
            };
            await _dbContext.Comments.AddAsync(commentToSave);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreatePost(PostRequest post)
        {
            Post postToSave = new Post() { 
            Title = post.Title,
            Contents = post.Contents,
            CreationDate = DateTime.UtcNow,
            ModificationDate = DateTime.UtcNow,
            CreationUserId = post.CreationUserId,
            ModificationUserId = post.CreationUserId,
            Likes = 0,
            Dislikes = 0,
            Status = (int)StatusEnums.PendingApproval,
            Views = 0
            };

            await _dbContext.Posts.AddAsync(postToSave);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteComment(decimal commentId)
        {
            Comment commentToDisable = _dbContext.Comments
                .SingleOrDefault(x => x.CommentId == commentId && x.Status == (int)StatusEnums.Published);
            if (commentToDisable == null)
                throw new ArgumentNullException(String.Format("The comment you are trying to delete does not exist"));
            commentToDisable.Status = (int)StatusEnums.Deleted;
            _dbContext.Comments.Update(commentToDisable);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePost(decimal postId)
        {
            Post postToDisable = _dbContext.Posts
                .SingleOrDefault(x => x.PostId == postId && x.Status != (int)StatusEnums.Deleted);
            if (postToDisable == null)
                throw new ArgumentNullException(String.Format("The post you are trying to delete does not exist"));
            postToDisable.Status = (int)StatusEnums.Deleted;
            _dbContext.Posts.Update(postToDisable);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DislikeComment(decimal commentId)
        {
            Comment commentToUpdate = await _dbContext.Comments.SingleOrDefaultAsync(x => x.CommentId == commentId && x.Status == (int)StatusEnums.Published);
            if (commentToUpdate == null)
                throw new ArgumentNullException(String.Format("The comment with the id: {0} doesn't exist", commentId));
            commentToUpdate.Dislikes += 1;
            _dbContext.Comments.Update(commentToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DislikePost(decimal postId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId && x.Status == (int)StatusEnums.Published);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be Approved", postId));
            postToUpdate.Dislikes += 1;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _dbContext.Posts.Include("CreationUser")
                .OrderByDescending(x => x.PostId)
                .Where(x => x.Status == (int)StatusEnums.Published)
                .Select(x => new Post() { 
                    PostId = x.PostId,
                    Title = x.Title,
                    Contents = x.Contents.Substring(0,100)+"...",
                    CreationDate = x.CreationDate,
                    ModificationDate = x.ModificationDate,
                    Likes = x.Likes,
                    Dislikes = x.Dislikes,
                    Views = x.Views,
                    CreationUser = new User() { 
                        UserId = x.CreationUser.UserId, 
                        Name = x.CreationUser.Name, 
                        Username = x.CreationUser.Username
                    }
                })
                .ToListAsync();
        }

        public async Task<List<Post>> GetPendingPosts()
        {
            return await _dbContext.Posts.Include("CreationUser")
                .OrderByDescending(x => x.PostId)
                .Where(x => x.Status == (int)StatusEnums.PendingApproval)
                .Select(x => new Post()
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    Contents = x.Contents.Substring(0, 100) + "...",
                    CreationDate = x.CreationDate,
                    ModificationDate = x.ModificationDate,
                    CreationUser = new User()
                    {
                        UserId = x.CreationUser.UserId,
                        Name = x.CreationUser.Name,
                        Username = x.CreationUser.Username
                    }
                })
                .ToListAsync();
        }

        public async Task<Post> GetPostById(decimal postId)
        {
            Post post = _dbContext.Posts
                .Include("Comments")
                .Include("Comments.CreationUser")
                .Include("CreationUser")
                .SingleOrDefault(x => x.PostId == postId && x.Status == (int)StatusEnums.Published);

            post.Views += 1;
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();

            if (post.CreationUser != null)
            {
                post.CreationUser.Password = null;
                post.CreationUser.Email = null;
                post.CreationUser.PostCreationUsers = null;
            }
            if(post.ModificationUser != null)
            {
                post.ModificationUser.Password = null;
                post.ModificationUser.Email = null;
                post.ModificationUser.PostModificationUsers = null;
            }
            foreach (Comment comment in post.Comments)
            {
                if (comment.CreationUser != null)
                {
                    comment.CreationUser.Password = null;
                    comment.CreationUser.Email = null;
                    comment.CreationUser.PostCreationUsers = null;
                    comment.CreationUser.CommentCreationUsers = null;
                }
            }
            if (post == null)
            {
                throw new ArgumentNullException(String.Format("There is no post with the id: {0}", postId));
            }
            return post;
        }

        public Task<List<Post>> GetPostsByUser()
        {
            throw new NotImplementedException();
        }

        public async Task LikeComment(decimal commentId)
        {
            Comment commentToUpdate = await _dbContext.Comments.SingleOrDefaultAsync(x => x.CommentId == commentId && x.Status == (int)StatusEnums.Published);
            if (commentToUpdate == null)
                throw new ArgumentNullException(String.Format("The comment with the id: {0} doesn't exist", commentId));
            commentToUpdate.Likes += 1;
            _dbContext.Comments.Update(commentToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LikePost(decimal postId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId && x.Status == (int)StatusEnums.Published);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist", postId));
            postToUpdate.Likes += 1;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RejectPost(decimal postId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId && x.Status == (int)StatusEnums.PendingApproval);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be Rejected", postId));
            postToUpdate.ModificationDate = DateTime.UtcNow;
            postToUpdate.Status = (int)StatusEnums.Rejected;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePost(PostRequest post)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == post.PostId && x.Status == (int)StatusEnums.Rejected);
            if(postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be modified", post.PostId));
            postToUpdate.Title = post.Title;
            postToUpdate.Contents = post.Contents;
            postToUpdate.ModificationUserId = post.ModificationUserId;
            postToUpdate.ModificationDate = DateTime.UtcNow;
            postToUpdate.Status = (int)StatusEnums.PendingApproval;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
