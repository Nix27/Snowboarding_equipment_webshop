using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task UpdateUserAsync(User userForUpdate)
        {
            var userFromDb = await dbSet.FirstOrDefaultAsync(u => u.Id == userForUpdate.Id);

            if (userFromDb != null)
            {
                userFromDb.UserName = userForUpdate.UserName;
                userFromDb.Email = userForUpdate.Email;
                userFromDb.Name = userForUpdate.Name;
                userFromDb.Phone = userForUpdate.Phone;
                userFromDb.StreetAddress = userForUpdate.StreetAddress;
                userFromDb.City = userForUpdate.City;
                userFromDb.PostalCode = userForUpdate.PostalCode;
                userFromDb.CountryId = userForUpdate.CountryId;
            }
        }
    }
}
