using System;
using System.Collections.Generic;
using System.Linq;
using apigateway.Models;

namespace apigateway
{
    public interface IPostsService
    {
        void AddPost(Post post);

        IList<Post> ReadPosts();

        void LikePost(Guid postId);

        void DislikePost(Guid postId);

        void DeletePost(Guid postId);
    }

    public class PostsService: IPostsService
    {
        private readonly IList<Post> _posts = new List<Post>();


        public void AddPost(Post post)
        {
            _posts.Add(post);
        }

        public IList<Post> ReadPosts()
        {
            return _posts;
        }

        public void LikePost(Guid postId)
        {
            var likedPost = _posts.Single(e => e.Id == postId);

            likedPost.Likes++;
        }

        public void DislikePost(Guid postId)
        {
            var dislikedPost = _posts.Single(e => e.Id == postId);

            if (dislikedPost.Likes <= 0) return;

            dislikedPost.Likes--;
        }

        public void DeletePost(Guid postId)
        {
            var postToRemove = _posts.Single(e => e.Id == postId);

            _posts.Remove(postToRemove);
        }
    }
}