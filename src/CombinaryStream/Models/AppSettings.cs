using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombinaryStream.Models
{
    public class AppSettings
    {
        public string YoutubeConnectionString { get; set; }
        public int YoutubeLimit { get; set; }
        public string TwitterConnectionString { get; set; }
        public int TwitterLimit { get; set; }
        public int CacheTtl { get; set; } = 60;
        public bool AutoRefreshCache { get; set; } = true;
    }
}
