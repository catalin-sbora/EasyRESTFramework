using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity:UniqueObject
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        bool Remove(TEntity entity);
        bool Update(TEntity entity); 
    }
}
