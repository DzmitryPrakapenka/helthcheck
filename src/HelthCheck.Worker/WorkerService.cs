using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelthCheck.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelthCheck.Worker
{
    public sealed class WorkerService : IHostedService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly ApplicationContext _applicationContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<JobListItem> _cronJobs;

        public WorkerService(ILogger<WorkerService> logger,
            ApplicationContext applicationContext,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _applicationContext = applicationContext;
            _serviceProvider = serviceProvider;
            _cronJobs = new List<JobListItem>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var checks = await _applicationContext.Checks
                .Where(c => c.Status == CheckStatus.Active)
                .Select(c => new
                {
                    Id = c.Id,
                    Host = c.TargetHost.IP,
                    Url = c.HelthCheckUrl,
                    Cron = c.Cron
                })
                .ToArrayAsync();

            foreach (var check in checks)
            {
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

                var task = Task.Factory.StartNew(() =>
                {
                    var checkJob = _serviceProvider.GetRequiredService<ICronJob>();

                    checkJob.ScheduleJob(check.Cron,
                        $"{check.Host}/{check.Url}",
                        check.Id,
                        cancelTokenSource.Token);

                    checkJob.Start();
                }, cancelTokenSource.Token);

                _cronJobs.Add(new JobListItem()
                {
                    CheckId = check.Id,
                    CancellationTokenSource = cancelTokenSource
                });
            }

            _logger.LogInformation("Worker has been started");
            //while (Console.ReadKey(true).Key == ConsoleKey.Escape)
            //{
            //    var item = _cronJobs.FirstOrDefault();

            //    item.CancellationTokenSource.Cancel();
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker has been stoped");
            return Task.CompletedTask;
        }
    }
}
