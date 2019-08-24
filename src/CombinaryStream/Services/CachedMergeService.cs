using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CombinaryStream.Services
{
    public class CachedMergeService : IItemRepository
    {
        private readonly IMemoryCache _cache;
        private readonly MergeService _mergeService;
        private readonly TimeSpan? _ttl;

        public CachedMergeService(IMemoryCache cache, MergeService mergeService, IConfiguration configuration) {
            _cache = cache;
            _mergeService = mergeService;
            var ttlSec = int.Parse(configuration["CacheTtl"] ?? "0");
            if (ttlSec > 0) {
                _ttl = TimeSpan.FromSeconds(ttlSec);
            } else {
                _ttl = null;
            }
        }

        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            var (items, _) = await GetItemsExAsync();
            return items;
        }

        public async Task<(IEnumerable<StreamItem> items, bool cacheHit)> GetItemsExAsync() {
            if (_ttl is null) {
                return (await _mergeService.GetItemsAsync(), false);
            }
            const string key = "StreamItems";
            var cacheHit = true;
            var items = await _cache.GetOrCreateAsync(key, async (entry) => {
                entry.AbsoluteExpirationRelativeToNow = _ttl;
                cacheHit = false;
                return await _mergeService.GetItemsAsync();
            });
            return (items, cacheHit);
        }
    }
}