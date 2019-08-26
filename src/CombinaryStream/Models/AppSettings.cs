using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombinaryStream.Models
{
    public class AppSettings
    {
        public string RssFeedUrl { get; set; }
        public int RssFeedLimit { get; set; } = 100;
        public string YoutubeConnectionString { get; set; }
        public int YoutubeLimit { get; set; } = 100;
        public string TwitterConnectionString { get; set; }
        public int TwitterLimit { get; set; } = 100;
        public string FacebookConnectionString { get; set; }
        public int FacebookLimit { get; set; } = 100;
        public int CacheTtl { get; set; } = 1800;
        public bool AutoRefreshCache { get; set; } = true;
    }
}
