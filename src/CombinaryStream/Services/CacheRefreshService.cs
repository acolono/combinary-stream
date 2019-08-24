using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CombinaryStream.Services {
    public class CacheRefreshService : IHostedService {
        private readonly ILogger<CacheRefreshService> _logger;
        private readonly IServiceProvider _services;
        private int _delay;
        private readonly bool _enabled;
        private Timer _timer;
        private readonly int _cacheTtl;

        public CacheRefreshService(AppSettings settings, IServiceProvider services, ILogger<CacheRefreshService> logger) {
            _logger = logger;
            _enabled = settings.AutoRefreshCache;
            if (_enabled) {
                _services = services;
                _delay = settings.CacheTtl - 1;
                if (_delay < 1) _delay = 1;
                _cacheTtl = settings.CacheTtl;
            }
        }
        public Task StartAsync(CancellationToken cancellationToken) {
            if (_enabled) _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_delay));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private void DoWork(object _) {
            var sw = new Stopwatch();
            sw.Start();
            using (var scope = _services.CreateScope()) {
                var cms = scope.ServiceProvider.GetService<CachedMergeService>();
                var count = cms.RefreshCache().GetAwaiter().GetResult();
                _logger.LogDebug($"Refreshing Cache (items={count})");
            }
            sw.Stop();
            var newDelay = _cacheTtl - (int)Math.Ceiling(sw.Elapsed.TotalSeconds);
            if (newDelay < 1) newDelay = 1;
            if (_delay != newDelay) {
                _delay = newDelay;
                var timespan = TimeSpan.FromSeconds(_delay);
                _timer.Change(timespan, timespan);
                _logger.LogDebug($"New delay: {_delay}sec (processing time={sw.Elapsed})");
            }
        }
    }
}