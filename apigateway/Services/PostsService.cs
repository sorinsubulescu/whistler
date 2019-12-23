using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public interface IPostsService
    {
        Task AddPost(Post post, CancellationToken cancellationToken = default);

        Task<IEnumerable<Post>> ReadPosts(CancellationToken cancellationToken = default);

        Task LikePost(string postId, CancellationToken cancellationToken = default);

        Task DislikePost(string postId, CancellationToken cancellationToken = default);

        Task DeletePost(string postId, CancellationToken cancellationToken = default);
    }

    public class PostsService: IPostsService
    {
        private readonly IPostProvider _postProvider;

        public PostsService(IPostProvider postProvider)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task AddPost(Post post, CancellationToken cancellationToken = default)
        {
            await _postProvider.AddPost(post, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Post>> ReadPosts(CancellationToken cancellationToken = default)
        {
            return await _postProvider.GetAll(cancellationToken).ConfigureAwait(false);
        }

        public async Task LikePost(string postId, CancellationToken cancellationToken = default)
        {
            await _postProvider.LikePost(postId, cancellationToken).ConfigureAwait(false);
        }

        public async Task DislikePost(string postId, CancellationToken cancellationToken = default)
        {
            await _postProvider.DislikePost(postId, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeletePost(string postId, CancellationToken cancellationToken = default)
        {
            await _postProvider.DeletePost(postId, cancellationToken).ConfigureAwait(false);
        }
    }
}