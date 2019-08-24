using System;
using System.Threading;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CombinaryStream.Services {
    public class CacheRefreshService : IHostedService {
        private readonly IServiceProvider _services;
        private readonly int _delay;
        private readonly bool _enabled;
        private Timer _timer;
        public CacheRefreshService(AppSettings settings, IServiceProvider services) {
            _enabled = settings.AutoRefreshCache;
            if (_enabled) {
                _services = services;
                _delay = settings.CacheTtl - 1;
                if (_delay < 1) _delay = 1;
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
            using (var scope = _services.CreateScope()) {
                IItemRepository cms = scope.ServiceProvider.GetService<CachedMergeService>();
                var items = cms.GetItemsAsync().GetAwaiter().GetResult();
            }
        }
    }
}