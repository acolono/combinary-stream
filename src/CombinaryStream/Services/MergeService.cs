using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CombinaryStream.Models;

namespace CombinaryStream.Services
{
    public class MergeService : IItemRepository
    {
        private readonly TwitterRepository _twitterRepository;
        private readonly YoutubeRepository _youtubeRepository;
        public MergeService(TwitterRepository twitterRepository, YoutubeRepository youtubeRepository) {
            _twitterRepository = twitterRepository;
            _youtubeRepository = youtubeRepository;
        }

        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            var result = new List<StreamItem>();
            var youtubeTask = _youtubeRepository.GetItemsAsync();
            var twitterTask = _twitterRepository.GetItemsAsync();
            result.AddRange(await twitterTask);
            result.AddRange(await youtubeTask);
            return result.OrderByDescending(r => r.PublishedAt);
        }
    }
}
