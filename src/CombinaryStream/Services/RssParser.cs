using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using CombinaryStream.Models;

namespace CombinaryStream.Services
{
    public class RssParser : IItemRepository
    {
        private List<RssSettings> _settings;
        public RssParser(AppSettings settings) {
            _settings = settings.Rss;
        }

        private const string MediaNamespace = "http://search.yahoo.com/mrss/";
        private const string DcNamespace = "http://purl.org/dc/elements/1.1/";

        public Task<List<StreamItem>> GetFeedAsync(string feedUrl, int limit) {
            var items = new List<StreamItem>();
            if(string.IsNullOrWhiteSpace(feedUrl) || limit <= 0) return Task.FromResult(items);
            using (var reader = XmlReader.Create(feedUrl)) {
                var feed = SyndicationFeed.Load(reader);
                foreach (var i in feed.Items) {
                    var item = new StreamItem {
                        Url = i.Links.FirstOrDefault()?.Uri?.ToString(),
                        ItemType = "rss",
                        Body = i.Summary?.Text,
                        Title = i.Title?.Text,
                        PublishedAt = i.LastUpdatedTime > i.PublishDate ? i.LastUpdatedTime : i.PublishDate,
                        AuthorName = i.Authors.FirstOrDefault()?.Name,
                        AuthorUrl = i.Authors.FirstOrDefault()?.Uri,
                    };

                    var mediaReader = i.ElementExtensions.FirstOrDefault(x => x.OuterName == "content" && x.OuterNamespace == MediaNamespace)?.GetReader();
                    if (mediaReader != null) {
                        var url = mediaReader.GetAttribute("url");
                        var medium = mediaReader.GetAttribute("medium");
                        if (medium == "image") item.ThumbnailUrl = url;
                    }

                    if (string.IsNullOrWhiteSpace(item.AuthorName)) {
                        var dcCreatorReader = i.ElementExtensions.FirstOrDefault(x => x.OuterName == "creator" && x.OuterNamespace == DcNamespace)?.GetReader();
                        if (dcCreatorReader != null) {
                            if(dcCreatorReader.Read()) item.AuthorName = dcCreatorReader.Value;
                        }
                    }

                    var dcDateReader = i.ElementExtensions.FirstOrDefault(x => x.OuterName == "date" && x.OuterName == DcNamespace)?.GetReader();
                    if (dcDateReader != null && dcDateReader.Read() && DateTimeOffset.TryParse(dcDateReader.Value, out var dcDate)) {
                        if (dcDate > item.PublishedAt) item.PublishedAt = dcDate;
                    }
                    

                    items.Add(item);
                    if(limit-- <= 0) break;
                }
            }

            return Task.FromResult(items);
        }
        
        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            var items = new List<StreamItem>();
            if (_settings == null || !_settings.Any()) return items;

            var tasks = _settings.Select(s => GetFeedAsync(s.FeedUrl, s.Limit)).ToList();
            foreach (var task in tasks) {
                items.AddRange(await task);
            }

            return items;
        }

    }
}
