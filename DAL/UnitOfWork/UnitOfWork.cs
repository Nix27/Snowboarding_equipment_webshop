using DAL.AppDbContext;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
