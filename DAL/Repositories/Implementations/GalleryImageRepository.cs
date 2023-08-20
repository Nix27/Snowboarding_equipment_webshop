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
    public class GalleryImageRepository : Repository<GalleryImage>, IGalleryImageRepository
    {
        public GalleryImageRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
