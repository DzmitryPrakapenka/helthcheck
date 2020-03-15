using System.Threading;
using System.Threading.Tasks;

namespace HelthCheck.Worker
{
    public interface ICronJob
    {
        int CheckId { get; }

        string CheckUrl { get; }

        void ScheduleJob(string cron, string url, int checkId);

        void Start(CancellationToken cancellationToken = default);
    }
}
