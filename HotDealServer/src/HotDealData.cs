namespace HotDealServer;

public record HotDealData(int Id, string Title, int UpVote, int DownVote, DateTime CreatedAt, bool SoldOut, int CommentCount);
