﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate=null,string? includeProperties=null);
        T GetT(Expression <Func<T, bool>> predicate, string? includeProperties = null);   

        void Add(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entity);
    }
}
