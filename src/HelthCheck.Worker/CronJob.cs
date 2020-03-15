using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using HelthCheck.Web.Data;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace HelthCheck.Worker
{
    public class CronJob : ICronJob
    {
        private readonly ILogger<CronJob> _logger;
        private readonly ApplicationContext _applicationContext;
        private CronExpression _expression;
        private Timer _timer;
        private readonly TimeZoneInfo _timeZoneInfo;

        public CronJob(ILogger<CronJob> logger,
            ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
            _timeZoneInfo = TimeZoneInfo.Local;
        }

        public int CheckId { get; private set; }

        public string CheckUrl { get; private set; }

        public void ScheduleJob(string cron, string checkUrl, int checkId)
        {
            CheckId = checkId;
            CheckUrl = checkUrl;
            _expression = CronExpression.Parse(cron);
        }

        public void Start(CancellationToken cancellationToken = default)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                _timer = new Timer(delay.TotalMilliseconds);

                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Stop();
                    await DoWork(cancellationToken);
                    Start(cancellationToken);
                };
                _timer.Start();
            }
        }

        private Task DoWork(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(CheckUrl);

            return Task.CompletedTask;
        }
    }
}
