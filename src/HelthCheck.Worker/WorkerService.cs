using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelthCheck.Web.Data;
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
        private readonly List<ICronJob> _cronJobs;

        public WorkerService(ILogger<WorkerService> logger,
            ApplicationContext applicationContext,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _applicationContext = applicationContext;
            _serviceProvider = serviceProvider;
            _cronJobs = new List<ICronJob>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var checks = await _applicationContext.Checks
                .Include(c => c.TargetHost)
                .ToArrayAsync();

            foreach (var check in checks)
            {
                var checkJob = _serviceProvider.GetRequiredService<ICronJob>();

                checkJob.ScheduleJob(check.Cron, $"{check.TargetHost.IP}/{check.HelthCheckUrl}", check.Id);

                checkJob.Start();

                _cronJobs.Add(checkJob);
            }

            _logger.LogInformation("Worker started");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
