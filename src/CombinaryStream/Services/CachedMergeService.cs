using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CombinaryStream.Services
{
    public class CachedMergeService : IItemRepository
    {
        private readonly IMemoryCache _cache;
        private readonly MergeService _mergeService;
        private readonly TimeSpan? _ttl;

        public CachedMergeService(IMemoryCache cache, MergeService mergeService, AppSettings settings) {
            _cache = cache;
            _mergeService = mergeService;
            
            if (settings.CacheTtl > 0) {
                _ttl = TimeSpan.FromSeconds(settings.CacheTtl);
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