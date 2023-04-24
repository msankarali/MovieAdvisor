using System.Linq.Expressions;
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

    public void ScheduleContinuationJob<TJob, TContinuationJob>(Expression<Action<TJob>> func,
                                                                Expression<Action<TContinuationJob>> continuationFunc)
    {
        string jobId = _backgroundJobClient.Enqueue<TJob>(func);
        _backgroundJobClient.ContinueJobWith<TContinuationJob>(jobId, continuationFunc);
    }

    public void ScheduleDelayedJob<TJob>(Expression<Action<TJob>> func, TimeSpan delay)
    {
        _backgroundJobClient.Schedule<TJob>(func, delay);
    }

    public void ScheduleFireAndForgetJob<TJob>(Expression<Action<TJob>> func)
    {
        _backgroundJobClient.Enqueue<TJob>(func);
    }

    public void ScheduleRecurringJob<TJob>(Expression<Action<TJob>> func, string cronExpression)
    {
        RecurringJob.AddOrUpdate(func, cronExpression);
    }
}
