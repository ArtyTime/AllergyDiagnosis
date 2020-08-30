using Allergy.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Allergy.Services
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        DbSet<TEntity> Entities();

        IQueryable<TEntity> GetAll();

        TEntity Get(int id);

        void Create(TEntity entity);

        Task CreateAsync(TEntity entity);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        //void DetectChanges();

        void Commit();

        Task CommitAsync();
    }
}
