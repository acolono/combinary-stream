using System.Collections.Generic;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Dapper;
using Npgsql;

namespace CombinaryStream.Services {
    public class TwitterRepository : IItemRepository {
        private readonly string _connectionString;
        private readonly int _limit;
        public TwitterRepository(AppSettings settings) {
            _connectionString = settings.TwitterConnectionString;
            _limit = settings.TwitterLimit;
        }
        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            if (string.IsNullOrWhiteSpace(_connectionString) || _limit <= 0) return new StreamItem[0];

            const string query = @"
                select
                       t.url as ""Url"",
                       'twitter' as ""ItemType"",
                       t.text as ""Body"",
                       t.media_photo as ""ThumbnailUrl"",
                       u.name || ' (@' || u.screen_name || ')' as ""AuthorName"",
                       'https://twitter.com/' || u.screen_name as ""AuthorUrl"",
                       t.ts as ""PublishedAt""

                from db.tweet t
                left join db.user u on t.user_id = u.id
                order by t.ts desc
                limit @limit
            ";

            using (var db = new NpgsqlConnection(_connectionString)) {
                return await db.QueryAsync<StreamItem>(query, new {limit = _limit});
            }
        }
    }
}
