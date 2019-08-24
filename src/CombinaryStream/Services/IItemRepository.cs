using System.Collections.Generic;
using System.Threading.Tasks;
using CombinaryStream.Models;

namespace CombinaryStream.Services
{
    public interface IItemRepository {
        Task<IEnumerable<StreamItem>> GetItemsAsync();
    }
}
