using System.Threading;

namespace HelthCheck.Worker
{
    public interface ICronJob
    {
        int CheckId { get; }

        string CheckUrl { get; }

        void ScheduleJob(string cron, string url, int checkId, CancellationToken cancellationToken);

        void Start();
    }
}
