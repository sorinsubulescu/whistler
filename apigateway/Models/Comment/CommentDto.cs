namespace apigateway
{
    public class CommentDto
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public string OwnerFullName { get; set; }

        public string OwnerProfilePictureFileName { get; set; }

        public string Message { get; set; }
    }
}