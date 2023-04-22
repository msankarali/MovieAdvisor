namespace Domain.Entities;

public class Rating : BaseEntity
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public string? Comment { get; set; }

    public virtual User User { get; set; }
}
