namespace HotDealServer.Models
{
    public record HotDealProduct
    {
        public string ProductName { get; set; }
        public string Href { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public CommunityType CommunityType { get; set; }
    }
}