namespace PostsCommentsService.Application.Feature.Comments.Query.Results
{
    public class GetCommentsByPostResult
    {
        public string Id { get; set; }
        public string PostId { get; set; }


        public string? ParentCommentId { get; set; }


        public Guid UserId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; }

        public int LikesCount { get; set; }
        public int RepliesCount { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
