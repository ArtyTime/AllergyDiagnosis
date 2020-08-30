using Allergy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Allergy.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        public readonly AllergiesDbContext Context;
        protected readonly IConfiguration Configuration;

        protected BaseService(AllergiesDbContext context, IConfiguration configuration)
        {
            Context = context;
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            Configuration = configuration;
        }

        protected virtual string TableName { get; set; }

        public DbSet<TEntity> Entities()
        {
            return Context.Set<TEntity>();
        }

        public void Reload(TEntity entity, string propertyName)
        {
            Context.Entry(entity).Reload();
            Context.Entry(entity).Reference(propertyName).IsLoaded = false;
            Context.SaveChanges();
            Context.Entry(entity).Reference(propertyName).Load();
        }

        public TEntity Reload(TEntity element)
        {
            Context.Entry(element).State = EntityState.Detached;

            element = Context.Find<TEntity>(element.Id);

            return element;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Entities().AsQueryable();
        }

        public TEntity Get(int id)
        {
            return Entities().FirstOrDefault(e => e.Id == id);
        }

        public void Create(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities().Add(entity);

            Commit();
        }

        public async Task CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities().Add(entity);

            await CommitAsync();
        }

        public virtual void Update(TEntity entity)
        {
            try
            {
                Commit();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();

                Commit();
            }
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            try
            {
                await CommitAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                e.Entries.Single().Reload();

                await CommitAsync();
            }
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities().Remove(entity);

            Commit();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities().Remove(entity);

            await CommitAsync();
        }

        public void Commit()
        {
            Context.ChangeTracker.DetectChanges();

            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            Context.ChangeTracker.DetectChanges();

            await Context.SaveChangesAsync();
        }
    }
}
