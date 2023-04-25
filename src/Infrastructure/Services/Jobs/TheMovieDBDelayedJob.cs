using System.Diagnostics;
using Application.Common.Jobs;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBDelayedJob : ITheMovieDBContinuationJob
{
    public void Trigger()
    {
        Debug.WriteLine("Runs at a specified time in the future.");
    }
}