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
            var items = new List<StreamItem>();
            using (var conn = new NpgsqlConnection(_connectionString)) {
                items.AddRange(await conn.QueryAsync<StreamItem>("SELECT * FROM items LIMIT @limit", new {limit=_limit}));
            }
            return items;
        }
    }
}
