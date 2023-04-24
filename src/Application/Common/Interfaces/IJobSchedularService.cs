using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IJobSchedulerService
{
    void ScheduleFireAndForgetJob<TJob>();
    void ScheduleDelayedJob<TJob>(TimeSpan delay);
    void ScheduleRecurringJob<TJob>(string cronExpression);
    void ScheduleContinuationJob<TJob>();
}

public interface IJob
{
    void Execute();
}