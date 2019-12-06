using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Dapper;
using Npgsql;

namespace CombinaryStream.Services
{
    public class FacebookRepository : IItemRepository
    {
        private readonly int _limit;
        private readonly string _connectionString;
        public FacebookRepository(AppSettings settings) {
            _connectionString = settings.FacebookConnectionString;
            _limit = settings.FacebookLimit;
        }
        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            if (string.IsNullOrWhiteSpace(_connectionString) || _limit <= 0) return new StreamItem[0];

            const string sql = @"
                select
                       'https://fb.com/' || p.page_id || '_' || p.id as ""Url"",
                       'facebook' as ""ItemType"",
                       p.story as ""Title"",
                       p.message as ""Body"",
                       p.created_time as ""PublishedAt"",
                       a.media_url as ""ThumbnailUrl""

                from post p
                join attachment a on a.id = (
                  select id from attachment ja
                  where ja.id = p.id
                  order by created_time desc
                  limit 1
                )
                order by p.created_time desc
                limit @limit
            ";

            var items = new List<StreamItem>();
            using (var conn = new NpgsqlConnection(_connectionString)) {
                items.AddRange(await conn.QueryAsync<StreamItem>(sql, new {limit=_limit}));
            }
            return items;
        }
    }
}
