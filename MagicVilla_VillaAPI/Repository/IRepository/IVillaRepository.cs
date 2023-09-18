using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        //Task é um tipo assíncrono

        Task<List<VillaModel>> GetAllAsync(Expression<Func<VillaModel, bool>> filter = null);

        Task<VillaModel> GetAsync(Expression<Func<VillaModel, bool>> filter = null, bool tracked=true);

        Task CreateAsync(VillaModel entity);
        
        Task UpdateAsync(VillaModel entity);
        
        Task RemoveAsync(VillaModel entity);
        
        Task SaveAsync();
    }
}
