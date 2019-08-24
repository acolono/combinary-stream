using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CombinaryStream.Services
{
    public class YoutubeRepository : IItemRepository
    {
        private readonly string _connectionString;
        private readonly int _limit;

        public YoutubeRepository(IConfiguration configuration) {
            _connectionString = configuration["YoutubeConnectionString"];
            _limit = int.Parse(configuration["YoutubeLimit"] ?? "100");
        }

        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            if(string.IsNullOrWhiteSpace(_connectionString)) return new StreamItem[0];

            const string query = @"
                select
                       'https://www.youtube.com/watch?v=' || v.""Id"" as ""Url"",
                       'youtube' as ""ItemType"",
                       v.""Title"",
                       v.""Description"" as ""Body"",
                       v.""MaxResImage"" as ""ThumbnailUrl"",
                       v.""ChannelTitle"" as ""AuthorName"",
                       'https://www.youtube.com/channel/' || v.""ChannelId"",
                       v.""PublishedAt""

                from ""Videos"" v
                order by v.""PublishedAt"" desc
                limit @limit
            ";

            using (var db = new NpgsqlConnection(_connectionString)) {
                return await db.QueryAsync<StreamItem>(query, param: new {limit = _limit});
            }
        }
    }
}
