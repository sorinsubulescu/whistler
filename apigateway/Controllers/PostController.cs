using System;
using System.Collections.Generic;
using apigateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace apigateway.Controllers
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
        public ActionResult<IList<Post>> Get()
        {
            return Ok(_postsService.ReadPosts());
        }

        [HttpPost]
        public ActionResult Post([FromBody] AddPostParameters addPostParameters)
        {
            _postsService.AddPost(new Post
            {
                Message = addPostParameters.Message
            });

            return Ok();
        }

        [HttpPut("like/{id}")]
        public ActionResult LikePost(string id)
        {
            if (!Guid.TryParse(id, out var parsedGuid))
                return BadRequest();

            _postsService.LikePost(parsedGuid);

            return Ok();
        }

        [HttpPut("dislike/{id}")]
        public ActionResult DislikePost(string id)
        {
            if (!Guid.TryParse(id, out var parsedGuid))
                return BadRequest();

            _postsService.DislikePost(parsedGuid);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (!Guid.TryParse(id, out var parsedGuid))
                return BadRequest();

            _postsService.DeletePost(parsedGuid);

            return Ok();
        }
    }
}