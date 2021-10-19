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

        public async Task ApprovePost(decimal postId, decimal userId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId 
            && x.Status == (int)StatusEnums.PendingApproval);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be Approved", postId));
            postToUpdate.ModificationDate = DateTime.UtcNow;
            postToUpdate.ModificationUserId = userId;
            postToUpdate.Status = (int)StatusEnums.Published;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateComment(CommentRequest comment, decimal userId)
        {
            int repeatedComments = _dbContext.Comments
                .Where(x => x.Status == (int)StatusEnums.Published 
                && x.PostId == comment.PostId 
                && x.CreationUser.UserId == userId 
                && x.Contents == comment.Contents).Count();
            Post post = _dbContext.Posts
                .SingleOrDefault(x => x.PostId == comment.PostId);
            User user = _dbContext.Users.Include("UsersRoles")
                .SingleOrDefault(x => x.UserId == userId);
            if (repeatedComments > 2)
                throw new DuplicateWaitObjectException("This comment couldn't be posted because you have already posted the same comment several times before.");
            if(post == null || (post.Status != (int)StatusEnums.Published && !user.UsersRoles.Any(x => x.RoleId == (int)RolesEnums.Editor)))
                throw new ArgumentNullException("The post where you are trying to comment does not exist");
            Comment commentToSave = new Comment()
            {
                Contents = comment.Contents,
                CreationUserId = userId,
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

        public async Task CreatePost(PostRequest post, decimal userId)
        {
            Post postToSave = new Post() { 
            Title = post.Title,
            Contents = post.Contents,
            CreationDate = DateTime.UtcNow,
            ModificationDate = DateTime.UtcNow,
            CreationUserId = userId,
            ModificationUserId = userId,
            Likes = 0,
            Dislikes = 0,
            Status = (int)StatusEnums.PendingApproval,
            Views = 0
            };

            await _dbContext.Posts.AddAsync(postToSave);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteComment(decimal commentId, decimal userId, bool isEditor)
        {
            Comment commentToDisable = _dbContext.Comments
                .SingleOrDefault(x => x.CommentId == commentId 
                && x.Status == (int)StatusEnums.Published
                &&(isEditor || x.CreationUserId == userId));
            if (commentToDisable == null)
                throw new ArgumentNullException(String.Format("The comment you are trying to delete does not exist"));
            commentToDisable.Status = (int)StatusEnums.Deleted;
            _dbContext.Comments.Update(commentToDisable);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePost(decimal postId, decimal userId, bool isEditor)
        {
            Post postToDisable = _dbContext.Posts
                .SingleOrDefault(x => x.PostId == postId 
                && x.Status != (int)StatusEnums.Deleted
                && (isEditor || x.CreationUserId == userId));
            if (postToDisable == null)
                throw new ArgumentNullException(String.Format("The post you are trying to delete does not exist"));
            postToDisable.Status = (int)StatusEnums.Deleted;
            _dbContext.Posts.Update(postToDisable);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DislikeComment(decimal commentId)
        {
            Comment commentToUpdate = await _dbContext.Comments.SingleOrDefaultAsync(x => x.CommentId == commentId 
            && x.Status == (int)StatusEnums.Published);
            if (commentToUpdate == null)
                throw new ArgumentNullException(String.Format("The comment with the id: {0} doesn't exist", commentId));
            commentToUpdate.Dislikes += 1;
            _dbContext.Comments.Update(commentToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DislikePost(decimal postId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId 
            && x.Status == (int)StatusEnums.Published);
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

        public async Task<Post> GetPostById(decimal postId, decimal userId, bool isEditor)
        {
            Post post = _dbContext.Posts
                .Include(x => x.Comments.Where(y => !y.IsReview && y.Status == (int)StatusEnums.Published))
                .Include("Comments.CreationUser")
                .Include("CreationUser")
                .SingleOrDefault(x => x.PostId == postId && ((x.CreationUserId != userId 
                && x.Status == (int)StatusEnums.Published) 
                || (x.CreationUserId == userId 
                || isEditor)));
            if (post == null)
                throw new ArgumentNullException(String.Format("There is no post with the id: {0}", postId));

            if (post.CreationUserId == userId || isEditor)
            {
               post.Comments.Concat(await _dbContext.Comments.Include("CreationUser").Where(x => x.IsReview && x.Status == (int)StatusEnums.Published).ToListAsync());
            }

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
            return post;
        }

        public async Task<List<Post>> GetPostsByUser(decimal userId)
        {
            return await _dbContext.Posts.Include("CreationUser")
                .OrderByDescending(x => x.PostId)
                .Where(x => x.CreationUserId == userId && x.Status != (int)StatusEnums.Deleted)
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

        public async Task LikeComment(decimal commentId)
        {
            Comment commentToUpdate = await _dbContext.Comments.SingleOrDefaultAsync(x => x.CommentId == commentId 
            && x.Status == (int)StatusEnums.Published
            && x.Post.Status == (int)StatusEnums.Published);
            if (commentToUpdate == null)
                throw new ArgumentNullException(String.Format("The comment with the id: {0} doesn't exist or the post with the comment does not exist", commentId));
            commentToUpdate.Likes += 1;
            _dbContext.Comments.Update(commentToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LikePost(decimal postId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId 
            && x.Status == (int)StatusEnums.Published);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist", postId));
            postToUpdate.Likes += 1;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RejectPost(decimal postId, decimal userId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.PostId == postId 
            && x.Status == (int)StatusEnums.PendingApproval);
            if (postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be Rejected", postId));
            postToUpdate.ModificationDate = DateTime.UtcNow;
            postToUpdate.ModificationUserId = userId;
            postToUpdate.Status = (int)StatusEnums.Rejected;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePost(PostRequest post, decimal userId)
        {
            Post postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(x => x.CreationUserId == userId 
            && x.PostId == post.PostId 
            && x.Status == (int)StatusEnums.Rejected);
            if(postToUpdate == null)
                throw new ArgumentNullException(String.Format("The post with the id: {0} doesn't exist or can't be modified", post.PostId));
            postToUpdate.Title = post.Title;
            postToUpdate.Contents = post.Contents;
            postToUpdate.ModificationUserId = userId;
            postToUpdate.ModificationDate = DateTime.UtcNow;
            postToUpdate.Status = (int)StatusEnums.PendingApproval;
            _dbContext.Posts.Update(postToUpdate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
