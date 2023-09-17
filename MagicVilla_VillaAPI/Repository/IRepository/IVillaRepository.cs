using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        //Task é um tipo assíncrono

        Task<List<VillaModel>> GetAll(Expression<Func<VillaModel>> filter = null);

        Task<VillaModel> Get(Expression<Func<VillaModel>> filter = null, bool tracked=true);

        Task Create(VillaModel entity);
        
        Task Remove(VillaModel entity);
        
        Task Save();
    }
}
