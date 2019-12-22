using System;

namespace apigateway.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; }

        public int Likes { get; set; } = 0;
    }
}