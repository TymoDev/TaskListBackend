using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.AbstractRepositories
{
    public class CrudAbstractions<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;
        private readonly DataContext context;
        protected CrudAbstractions(DataContext context,DbSet<TEntity> dbSet)
        {
            this.context = context;
            this.dbSet = dbSet;
        }
        public async Task<List<TResponce>> Get<TResponce>(Func<TEntity,TResponce> selector)
        {
            var entities = await dbSet.AsNoTracking().ToListAsync();
            return entities.Select(selector).ToList();
        }

        public async Task<TResponce> GetById<TResponce>(Guid id,Func<TEntity, TResponce> selector)
        {
            var entity = await dbSet.FindAsync(id);
            return selector(entity);
        }
        public async Task<Guid> Create(TEntity entity)
        {
           await dbSet.AddAsync(entity);
           await context.SaveChangesAsync();
           return ((dynamic)entity).Id; 
        }
        public async Task<Guid> Update(Guid id, Action<TEntity> updateAction)
        {
            var entity = await dbSet.FindAsync(id);

            updateAction(entity);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var entity = await dbSet.FindAsync(id);

            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return id;
        }
    }
}
