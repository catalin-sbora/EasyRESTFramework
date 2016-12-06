using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataModels;

namespace DataAccess.EntityFrameworkCore
{
    public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : UniqueObject
    {
        protected readonly StudentsDbContext _dbContext = null;
        protected readonly DbSet<TEntity> _dbSet = null;

        public EFGenericRepository(StudentsDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();            
            
            _dbContext.SaveChanges();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);            
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.FirstOrDefault(entity => entity.Id == id);
            //throw new NotImplementedException();
        }

        public bool Remove(TEntity entity)
        {
            return (_dbSet.Remove(entity) != null);
            //throw new NotImplementedException();
        }

        public bool Update(TEntity entity)
        {
            return (_dbSet.Update(entity) != null);
        }
    }
}
