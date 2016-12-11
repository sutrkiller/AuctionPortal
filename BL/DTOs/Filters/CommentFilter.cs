namespace BL.DTOs.Filters
{
    public class CommentFilter
    {
        public long ID { get; set; }
        public long AuctionId { get; set; }
        public long AuthorId { get; set; }
        public long? ParentId { get; set; }
        public string Text { get; set; }
    }
}
