using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using zmgTestBack.Filters;
using zmgTestBack.Models;
using zmgTestBack.Services;

namespace zmgTestBack.Controllers
{
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
        public async Task<IActionResult> GetPostById(decimal postId)
        {
            var result = await _serviceHandler.GetPostById(postId);
            return Ok(result);
        }

        [HttpGet("getPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _serviceHandler.GetAllPosts();
            return Ok(result);
        }

        [HttpGet("getPendingPosts")]
        public async Task<IActionResult> GetPendingPosts()
        {
            var result = await _serviceHandler.GetPendingPosts();
            return Ok(result);
        }

        [HttpGet("getMyPosts")]
        public async Task<IActionResult> GetPostsByUser()
        {
            var result = await _serviceHandler.GetPostsByUser();
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("createPost")]
        public async Task<IActionResult> CreatePost([FromBody] PostRequest post)
        {
            await _serviceHandler.CreatePost(post);
            return Ok();
        }
        [HttpPost("createComment")]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest comment)
        {
            await _serviceHandler.CreateComment(comment);
            return Ok();
        }
        #endregion

        #region PUT
        [HttpPut("updatePost")]
        public async Task<IActionResult> UpdatePost([FromBody] PostRequest post)
        {
            await _serviceHandler.UpdatePost(post);
            return Ok();
        }
        [HttpPut("rejectPost/{postId}")]
        public async Task<IActionResult> RejectPost(decimal postId)
        {
            await _serviceHandler.RejectPost(postId);
            return Ok();
        }
        [HttpPut("approvePost/{postId}")]
        public async Task<IActionResult> ApprovePost(decimal postId)
        {
            await _serviceHandler.ApprovePost(postId);
            return Ok();
        }
        [HttpPut("likePost/{postId}")]
        public async Task<IActionResult> LikePost(decimal postId)
        {
            await _serviceHandler.LikePost(postId);
            return Ok();
        }
        [HttpPut("dislikePost/{postId}")]
        public async Task<IActionResult> DislikePost(decimal postId)
        {
            await _serviceHandler.DislikePost(postId);
            return Ok();
        }
        [HttpPut("likeComment/{commentId}")]
        public async Task<IActionResult> LikeComment(decimal commentId)
        {
            await _serviceHandler.LikeComment(commentId);
            return Ok();
        }
        [HttpPut("dislikeComment/{commentId}")]
        public async Task<IActionResult> DislikeComment(decimal commentId)
        {
            await _serviceHandler.DislikeComment(commentId);
            return Ok();
        }

        #endregion

        #region DELETE
        [HttpDelete("deletePost/{postId}")]
        public async Task<IActionResult> DeletePost(decimal postId)
        {
            await _serviceHandler.DeletePost(postId);
            return Ok();
        }
        [HttpDelete("deleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(decimal commentId)
        {
            await _serviceHandler.DeleteComment(commentId);
            return Ok();
        }
        #endregion
    }
}
