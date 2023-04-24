using Application.Common.Interfaces;
using Hangfire;

namespace Infrastructure.Services;

public class HangfireJobSchedulerService : IJobSchedulerService
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireJobSchedulerService(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public void ScheduleContinuationJob<TJob>()
    {
        string jobId = _backgroundJobClient.Enqueue<IJob>(job => job.Execute());
        _backgroundJobClient.ContinueJobWith<IJob>(jobId, job => job.Execute());
    }

    public void ScheduleDelayedJob<TJob>(TimeSpan delay)
    {
        _backgroundJobClient.Schedule<IJob>(job => job.Execute(), delay);
    }

    public void ScheduleFireAndForgetJob<TJob>()
    {
        _backgroundJobClient.Enqueue<IJob>(job => job.Execute());
    }

    public void ScheduleRecurringJob<TJob>(string cronExpression)
    {
        RecurringJob.AddOrUpdate<IJob>(job => job.Execute(), cronExpression);
    }
}
