using System;
using System.Collections;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apigateway
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {

        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;

        public PostController(IQueryFactory queryFactory, ICommandFactory commandFactory)
        {
            _queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<PostDto>> GetPostById(string postId,
            CancellationToken cancellationToken = default)
        {
            var post = await _queryFactory.GetPostByIdQuery(postId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> Get(CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _queryFactory.GetPostsQuery(userId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok(posts);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<GetPostsDto>> GetPostsByUserId(string userId,
            CancellationToken cancellationToken = default)
        {
            var posts = await _queryFactory.GetPostsByUserIdQuery(userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddPostParameters addPostParameters,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _commandFactory.AddPostCommand(addPostParameters, userId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok();
        }


        [HttpPut("like/{postId}")]
        public async Task<ActionResult> LikePost(string postId, CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _commandFactory.LikePostCommand(postId, userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            switch (response.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPut("dislike/{postId}")]
        public async Task<ActionResult> DislikePost(string postId, CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _commandFactory.DislikePostCommand(postId, userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            switch (response.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> Delete(string postId, CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _commandFactory.DeletePostCommand(postId, userId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("comment/{postId}")]
        public async Task<ActionResult> AddComment([FromBody] AddCommentParameters addCommentParameters, string postId,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _commandFactory.AddCommentCommand(addCommentParameters, postId, userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            switch (response.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPut("delete_comment/{postId}")]
        public async Task<ActionResult> DeleteComment([FromBody] DeleteCommentParameters deleteCommentParameters, string postId,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _commandFactory.DeleteCommentCommand(deleteCommentParameters, postId, userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            switch (response.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}