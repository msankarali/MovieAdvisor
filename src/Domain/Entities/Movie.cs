namespace Domain.Entities;

public class Movie : BaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    
    public virtual List<Rating>? Ratings { get; set; }
}
