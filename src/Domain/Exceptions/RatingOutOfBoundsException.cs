namespace Domain.Exceptions;

public class RatingOutOfBoundsException : Exception
{
    public RatingOutOfBoundsException() : base("Rating must be between 0 to 10") { }
}