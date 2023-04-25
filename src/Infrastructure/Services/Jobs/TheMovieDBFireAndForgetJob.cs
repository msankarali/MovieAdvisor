using System.Diagnostics;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBFireAndForgetJob
{
    public void ExecuteAndForgetMe(string withSomeStringHere)
    {
        Debug.WriteLine("Runs whenever it is called with " + withSomeStringHere);
    }
}