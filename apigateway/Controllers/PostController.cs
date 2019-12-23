using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace apigateway
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IPostsService _postsService;

        public PostController(IPostsService postsService)
        {
            _postsService = postsService ?? throw new ArgumentNullException(nameof(postsService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> Get(CancellationToken cancellationToken = default)
        {
            var posts = await _postsService.ReadPosts(cancellationToken).ConfigureAwait(false);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddPostParameters addPostParameters,
            CancellationToken cancellationToken = default)
        {
            await _postsService.AddPost(
                new Post
                {
                    Message = addPostParameters.Message
                },
                cancellationToken).ConfigureAwait(false);

            return Ok();
        }


        [HttpPut("like/{id}")]
        public async Task<ActionResult> LikePost(string id, CancellationToken cancellationToken = default)
        {
            await _postsService.LikePost(id, cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("dislike/{id}")]
        public async Task<ActionResult> DislikePost(string id, CancellationToken cancellationToken = default)
        {
            await _postsService.DislikePost(id, cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await _postsService.DeletePost(id, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
    }
}