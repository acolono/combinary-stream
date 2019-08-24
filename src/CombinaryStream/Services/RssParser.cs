using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using CombinaryStream.Models;

namespace CombinaryStream.Services
{
    public class RssParser : IItemRepository
    {
        private readonly string _feedUrl;
        private readonly int _limit;

        public RssParser(AppSettings settings) {
            _feedUrl = settings.RssFeedUrl;
            _limit = settings.RssFeedLimit;
        }
        
        public IEnumerable<StreamItem> GetItems() {
            var items = new List<StreamItem>();
            if(string.IsNullOrWhiteSpace(_feedUrl) || _limit <= 0) return items;
            using (var reader = XmlReader.Create(_feedUrl)) {
                var limit = _limit;
                var feed = SyndicationFeed.Load(reader);
                foreach (var i in feed.Items) {
                    items.Add(new StreamItem {
                        Url = i.Links.FirstOrDefault()?.Uri?.ToString(),
                        ItemType = "rss",
                        Body = i.Summary?.Text,
                        Title = i.Title?.Text,
                        PublishedAt = i.LastUpdatedTime > i.PublishDate ? i.LastUpdatedTime : i.PublishDate,
                        AuthorName = i.Authors.FirstOrDefault()?.Name,
                        AuthorUrl = i.Authors.FirstOrDefault()?.Uri,
                    });
                    if(limit-- <= 0) break;
                }
            }

            return items;
        }
        
        public Task<IEnumerable<StreamItem>> GetItemsAsync() {
            return Task.FromResult(GetItems());
        }
    }
}
