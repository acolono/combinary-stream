using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombinaryStream.Models
{
    public class AppSettings
    {
        public string YoutubeConnectionString { get; set; }
        public int YoutubeLimit { get; set; } = 500;
        public string TwitterConnectionString { get; set; }
        public int TwitterLimit { get; set; } = 500;
        public int CacheTtl { get; set; } = 1800;
        public bool AutoRefreshCache { get; set; } = true;
    }
}
