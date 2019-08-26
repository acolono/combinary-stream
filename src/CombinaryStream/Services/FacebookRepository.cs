using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CombinaryStream.Models;

namespace CombinaryStream.Services
{
    public class FacebookRepository : IItemRepository
    {
        private int _limit;
        private string _connectionString;
        public FacebookRepository(AppSettings settings) {
            _connectionString = settings.FacebookConnectionString;
            _limit = settings.FacebookLimit;
        }
        public Task<IEnumerable<StreamItem>> GetItemsAsync() {
            return Task.FromResult(new StreamItem[0].AsEnumerable());
        }
    }
}
