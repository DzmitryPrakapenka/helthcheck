using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using HelthCheck.Data.Entities;
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
        private CancellationToken _cancellationToken;

        public CronJob(ILogger<CronJob> logger,
            ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
            _timeZoneInfo = TimeZoneInfo.Local;
        }

        public int CheckId { get; private set; }

        public string CheckUrl { get; private set; }

        public void ScheduleJob(string cron, string checkUrl, int checkId, CancellationToken cancellationToken)
        {
            CheckId = checkId;
            CheckUrl = checkUrl;
            _expression = CronExpression.Parse(cron);
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            if (!_cancellationToken.IsCancellationRequested)
            {
                var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

                if (next.HasValue)
                {
                    var delay = next.Value - DateTimeOffset.Now;

                    _timer = new Timer(delay.TotalMilliseconds);

                    _timer.Elapsed += async (sender, args) =>
                    {
                        _timer.Stop();
                        await DoWork();
                        Start();
                    };

                    _timer.Start();
                }
            }
            else
            {
                _timer.Stop();
                _timer.Dispose();
                return;
            }
        }

        private Task DoWork()
        {
            if (!_cancellationToken.IsCancellationRequested)
            {
                var thread = Thread.CurrentThread;

                var threadInfo = string.Format("Background: {0}\nThread Pool: {1}\nThread ID: {2}\n",
                    thread.IsBackground, thread.IsThreadPoolThread, thread.ManagedThreadId);

                _logger.LogInformation(threadInfo);
                _logger.LogInformation(CheckUrl);
            }

            return Task.CompletedTask;
        }
    }
}
