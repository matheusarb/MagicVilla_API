using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        
        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(VillaModel entity)
        {
            await _db.Villas.AddAsync(entity);
            Save();
        }

        public async Task<VillaModel> Get(Expression<Func<VillaModel, bool>> filter = null, bool tracked = true)
        {
            IQueryable<VillaModel> query = _db.Villas;

            if (!tracked)
            {
                query= query.AsNoTracking();
            }
            if(filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<VillaModel>> GetAll(Expression<Func<VillaModel, bool>> filter = null)
        {
            IQueryable<VillaModel> query = _db.Villas;

            if(filter != null)
            { 
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task Remove(VillaModel entity)
        {
            _db.Villas.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Save()
        {
            _db.SaveChangesAsync();
        }
    }
}
