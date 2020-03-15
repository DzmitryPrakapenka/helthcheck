using System.Threading;
using System.Threading.Tasks;

namespace HelthCheck.Worker
{
    public class JobListItem
    {
        public int CheckId { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
