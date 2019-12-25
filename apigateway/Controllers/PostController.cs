using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apigateway
{
    [Route("api/[controller]")]
    [ApiController]
//    [Authorize]
    public class PostController : ControllerBase
    {

        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;

        public PostController(IQueryFactory queryFactory, ICommandFactory commandFactory)
        {
            _queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> Get(CancellationToken cancellationToken = default)
        {
            var posts = await _queryFactory.GetPostsQuery().Execute(cancellationToken).ConfigureAwait(false);

            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddPostParameters addPostParameters,
            CancellationToken cancellationToken = default)
        {
            await _commandFactory.AddPostCommand(addPostParameters).Execute(cancellationToken).ConfigureAwait(false);

            return Ok();
        }


        [HttpPut("like/{postId}")]
        public async Task<ActionResult> LikePost(string postId, CancellationToken cancellationToken = default)
        {
            await _commandFactory.LikePostCommand(postId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("dislike/{postId}")]
        public async Task<ActionResult> DislikePost(string postId, CancellationToken cancellationToken = default)
        {
            await _commandFactory.DislikePostCommand(postId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> Delete(string postId, CancellationToken cancellationToken = default)
        {
            await _commandFactory.DeletePostCommand(postId).Execute(cancellationToken).ConfigureAwait(false);

            return Ok();
        }
    }
}