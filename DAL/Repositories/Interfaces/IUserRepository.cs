using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateUserAsync(User userForUpdate);
    }
}
