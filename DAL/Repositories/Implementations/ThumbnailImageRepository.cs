using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    public class ThumbnailImageRepository : Repository<ThumbnailImage>, IThumbnailImageRepository
    {
        public ThumbnailImageRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
