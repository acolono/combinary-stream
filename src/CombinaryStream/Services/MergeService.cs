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
        private readonly StreamItem _nag;
        private readonly int _nagEvery;


        public MergeService(TwitterRepository twitterRepository, YoutubeRepository youtubeRepository, RssParser rssParser, FacebookRepository facebookRepository, AppSettings appSettings) {
            _twitterRepository = twitterRepository;
            _youtubeRepository = youtubeRepository;
            _rssParser = rssParser;
            _facebookRepository = facebookRepository;
            _nagEvery = appSettings.NagEvery;
            _nag = appSettings.Nag;
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
            var sorted = result.OrderByDescending(r => r.PublishedAt);

            if (_nagEvery > 0 && _nag != null) {
                var i = 0;
                var nagAdded = false;
                var withNags = new List<StreamItem>();
                foreach (var item in sorted) {
                    i++;
                    if (i >= _nagEvery) {
                        i = 0;
                        withNags.Add(_nag);
                        nagAdded = true;
                    }
                    withNags.Add(item);
                }
                if(!nagAdded) withNags.Add(_nag);
                return withNags;
            }
            return sorted;
        }
    }
}
