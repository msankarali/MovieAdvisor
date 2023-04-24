using System.Diagnostics;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBDelayedJob
{
    public void Trigger()
    {
        Debug.WriteLine("Runs at a specified time in the future.");
    }
}