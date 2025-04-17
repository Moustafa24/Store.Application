using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace peresistence.Repositeries
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChange)
        {


            if (typeof(TEntity) == typeof(Product))
            {
                return trackChange ?
                  await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync() as IEnumerable<TEntity>
                : await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }


            return trackChange ?
                  await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();

            //if (trackChange) return await _context.Set<TEntity>().ToListAsync();

            //return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {

            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync(p=>p.Id == id as int?)as TEntity;
            }


                return await _context.Set<TEntity>().FindAsync(id);

        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);    
        }

        public void Update(TEntity entity)
        {

            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
           _context.Remove(entity);
        }



    }
}
