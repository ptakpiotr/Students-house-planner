using Hangfire;
using Server.Services;

namespace Server.Jobs
{
    public class BackgroundJobExecutor(IRecurringJobManager manager, IInitNextMonthService nextMonthService)
    {
        public void RegisterJobs()
        {
            manager.AddOrUpdate("next-month", () => nextMonthService.CreateNextMonthAsync(), Cron.Monthly(1, 5));
        }
    }
}