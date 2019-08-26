using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CombinaryStream.Models;

namespace CombinaryStream.Services
{
    public class MergeService : IItemRepository
    {
        private readonly IItemRepository _twitterRepository;
        private readonly IItemRepository _youtubeRepository;
        private readonly IItemRepository _rssParser;
        private readonly IItemRepository _facebookRepository;

        public MergeService(TwitterRepository twitterRepository, YoutubeRepository youtubeRepository, RssParser rssParser, FacebookRepository facebookRepository) {
            _twitterRepository = twitterRepository;
            _youtubeRepository = youtubeRepository;
            _rssParser = rssParser;
            _facebookRepository = facebookRepository;
        }

        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            var rssTask = _rssParser.GetItemsAsync();
            var youtubeTask = _youtubeRepository.GetItemsAsync();
            var twitterTask = _twitterRepository.GetItemsAsync();
            var facebookTask = _facebookRepository.GetItemsAsync();
            var result = new List<StreamItem>();
            result.AddRange(await twitterTask);
            result.AddRange(await youtubeTask);
            result.AddRange(await facebookTask);
            result.AddRange(await rssTask);
            return result.OrderByDescending(r => r.PublishedAt);
        }
    }
}
