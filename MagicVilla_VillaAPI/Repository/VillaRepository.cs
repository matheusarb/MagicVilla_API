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

        public async Task CreateAsync(VillaModel entity)
        {
            await _db.Villas.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<VillaModel> GetAsync(Expression<Func<VillaModel, bool>> filter = null, bool tracked = true)
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

        public async Task<List<VillaModel>> GetAllAsync(Expression<Func<VillaModel, bool>> filter = null)
        {
            IQueryable<VillaModel> query = _db.Villas;

            if(filter != null)
            { 
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(VillaModel entity)
        {
            _db.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(VillaModel entity)
        {
            _db.Villas.Update(entity);
            await SaveAsync();
        }
    }
}
