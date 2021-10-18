using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using zmgTestBack.Filters;
using zmgTestBack.Models;
using zmgTestBack.Services;

namespace zmgTestBack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [TypeFilter(typeof(BlogExceptionFilter))]
    public class BlogController : ControllerBase
    {
        private readonly IServiceHandler _serviceHandler;

        public BlogController(IServiceHandler serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        #region GET

        [HttpGet("getPost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> GetPostById(decimal postId)
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _serviceHandler.GetPostById(postId, userId);
            return Ok(result);
        }

        [HttpGet("getPosts")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _serviceHandler.GetAllPosts();
            return Ok(result);
        }

        [HttpGet("getPendingPosts")]
        [Authorize(Roles = RolesConstants.Editor)]
        public async Task<IActionResult> GetPendingPosts()
        {
            var result = await _serviceHandler.GetPendingPosts();
            return Ok(result);
        }

        [HttpGet("getMyPosts")]
        [Authorize(Roles = RolesConstants.Writer)]
        public async Task<IActionResult> GetPostsByUser()
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _serviceHandler.GetPostsByUser(userId);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("createPost")]
        [Authorize(Roles = RolesConstants.Writer)]
        public async Task<IActionResult> CreatePost([FromBody] PostRequest post)
        {
            await _serviceHandler.CreatePost(post);
            return Ok();
        }
        [HttpPost("createComment")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest comment)
        {
            await _serviceHandler.CreateComment(comment);
            return Ok();
        }
        #endregion

        #region PUT
        [HttpPut("updatePost")]
        [Authorize(Roles = RolesConstants.Writer)]
        public async Task<IActionResult> UpdatePost([FromBody] PostRequest post)
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _serviceHandler.UpdatePost(post, userId);
            return Ok();
        }
        [HttpPut("rejectPost/{postId}")]
        [Authorize(Roles = RolesConstants.Editor)]
        public async Task<IActionResult> RejectPost(decimal postId)
        {
            await _serviceHandler.RejectPost(postId);
            return Ok();
        }
        [HttpPut("approvePost/{postId}")]
        [Authorize(Roles = RolesConstants.Editor)]
        public async Task<IActionResult> ApprovePost(decimal postId)
        {
            await _serviceHandler.ApprovePost(postId);
            return Ok();
        }
        [HttpPut("likePost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> LikePost(decimal postId)
        {
            await _serviceHandler.LikePost(postId);
            return Ok();
        }
        [HttpPut("dislikePost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DislikePost(decimal postId)
        {
            await _serviceHandler.DislikePost(postId);
            return Ok();
        }
        [HttpPut("likeComment/{commentId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> LikeComment(decimal commentId)
        {
            await _serviceHandler.LikeComment(commentId);
            return Ok();
        }
        [HttpPut("dislikeComment/{commentId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DislikeComment(decimal commentId)
        {
            await _serviceHandler.DislikeComment(commentId);
            return Ok();
        }

        #endregion

        #region DELETE
        [HttpDelete("deletePost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DeletePost(decimal postId)
        {
            //TODO: Solo el dueño
            await _serviceHandler.DeletePost(postId);
            return Ok();
        }
        [HttpDelete("deleteComment/{commentId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DeleteComment(decimal commentId)
        {
            //TODO: Solo el dueño
            await _serviceHandler.DeleteComment(commentId);
            return Ok();
        }
        #endregion
    }
}
