namespace apigateway
{
    public class PostDto
    {
        public string Id { get; set; }
        public string Message { get; set; }

        public int LikeCount { get; set; }

        public UserDto Owner { get; set; }
    }
}