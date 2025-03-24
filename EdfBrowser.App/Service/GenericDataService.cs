using EdfBrowser.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdfBrowser.App
{
    internal class GenericDataService<T> where T : DomainObject
    {
        private readonly IAppDbContextFactory _contextFactory;

        public GenericDataService(IAppDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        internal async Task<T> Create(T item)
        {
            using (AppDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(item);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }


        internal async Task<T> Update(T item)
        {
            using (AppDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = context.Set<T>().Update(item);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        internal async Task<IEnumerable<T>> GetAll()
        {
            using (AppDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().ToListAsync();
            }
        }

    }
}
