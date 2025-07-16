using DataLayer.Entities;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IClassRepository
    {
        Task<Class> GetByIdAsync(int id);
    }
} 