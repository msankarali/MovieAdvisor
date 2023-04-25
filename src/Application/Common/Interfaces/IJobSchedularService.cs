using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IJobSchedulerService
{
    void ScheduleFireAndForgetJob<TJob>(Expression<Action<TJob>> func);
    void ScheduleDelayedJob<TJob>(Expression<Action<TJob>> func, TimeSpan delay);
    void ScheduleRecurringJob<TJob>(Expression<Action<TJob>> func, string cronExpression);
    void ScheduleContinuationJob<TJob, TContinuationJob>(Expression<Action<TJob>> func,
                                                         Expression<Action<TContinuationJob>> continuationFunc);
}
