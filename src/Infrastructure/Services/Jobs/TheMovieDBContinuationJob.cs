using System.Diagnostics;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBContinuationJob
{
    public void ExecuteContinuation()
    {
        Debug.WriteLine("Runs automatically after the execution of a parent job.");
    }
}