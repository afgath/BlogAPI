using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using zmgTestBack.Constants;
using zmgTestBack.Filters;
using zmgTestBack.Models;
using zmgTestBack.Models.Responses;
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
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _serviceHandler.CreatePost(post, userId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.Created, string.Format(ResponseConstants.Created, nameof(post)));
            return Ok(response);
        }
        [HttpPost("createComment")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest comment)
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _serviceHandler.CreateComment(comment, userId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Created, nameof(comment)));
            return Ok(response);
        }
        #endregion

        #region PUT
        [HttpPut("updatePost")]
        [Authorize(Roles = RolesConstants.Writer)]
        public async Task<IActionResult> UpdatePost([FromBody] PostRequest post)
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _serviceHandler.UpdatePost(post, userId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Updated, nameof(post)));
            return Ok(response);
        }
        [HttpPut("rejectPost/{postId}")]
        [Authorize(Roles = RolesConstants.Editor)]
        public async Task<IActionResult> RejectPost(decimal postId)
        {
            await _serviceHandler.RejectPost(postId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Reject, nameof(Post)));
            return Ok(response);
        }
        [HttpPut("approvePost/{postId}")]
        [Authorize(Roles = RolesConstants.Editor)]
        public async Task<IActionResult> ApprovePost(decimal postId)
        {
            await _serviceHandler.ApprovePost(postId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Approve, nameof(Post)));
            return Ok(response);
        }
        [HttpPut("likePost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> LikePost(decimal postId)
        {
            await _serviceHandler.LikePost(postId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Updated, nameof(Post)));
            return Ok(response);
        }
        [HttpPut("dislikePost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DislikePost(decimal postId)
        {
            await _serviceHandler.DislikePost(postId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Updated, nameof(Post)));
            return Ok(response);
        }
        [HttpPut("likeComment/{commentId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> LikeComment(decimal commentId)
        {
            await _serviceHandler.LikeComment(commentId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Updated, nameof(Comment)));
            return Ok(response);
        }
        [HttpPut("dislikeComment/{commentId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DislikeComment(decimal commentId)
        {
            await _serviceHandler.DislikeComment(commentId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Updated, nameof(Comment)));
            return Ok(response);
        }

        #endregion

        #region DELETE
        [HttpDelete("deletePost/{postId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DeletePost(decimal postId)
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _serviceHandler.DeletePost(postId, userId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Delete, nameof(Post)));
            return Ok(response);
        }
        [HttpDelete("deleteComment/{commentId}")]
        [Authorize(Roles = RolesConstants.Writer + "," + RolesConstants.Editor + "," + RolesConstants.Viewer)]
        public async Task<IActionResult> DeleteComment(decimal commentId)
        {
            decimal userId = decimal.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _serviceHandler.DeleteComment(commentId, userId);
            GenericResponse response = new GenericResponse((int)HttpStatusCode.OK, string.Format(ResponseConstants.Delete, nameof(Comment)));
            return Ok(response);
        }
        #endregion
    }
}
