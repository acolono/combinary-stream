using System.Collections.Generic;

namespace CombinaryStream.Models
{
    public class AppSettings
    {
        public string YoutubeConnectionString { get; set; }
        public int YoutubeLimit { get; set; } = 100;
        public string TwitterConnectionString { get; set; }
        public int TwitterLimit { get; set; } = 100;
        public string FacebookConnectionString { get; set; }
        public int FacebookLimit { get; set; } = 100;
        public List<long> FacebookPageIds { get; set; }
        public int CacheTtl { get; set; } = 1800;
        public bool AutoRefreshCache { get; set; } = true;
        public List<RssSettings> Rss { get; set; }
        public int NagEvery { get; set; } = 50;
        public StreamItem Nag { get; set; }
        public string DateFormat { get; set; } = "dd.MM.yyyy";
        public string ReadMoreText { get; set; } = "read more";
    }

    public class RssSettings {
        public string FeedUrl { get; set; }
        public int Limit { get; set; } = 100;
    }
}
