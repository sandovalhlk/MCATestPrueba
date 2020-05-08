using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Métodos genéricos para todos los repositorios
    /// </summary>
    public  interface IRepository<TEntity> where TEntity : class
    {
        /*** No asincronos ***/
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        TEntity GetByID(object id); 
        List<TEntity> GetAll();
        List<TEntity> GetAll(List<Expression<Func<TEntity, object>>> includes);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        TEntity Single(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, object>>> includes);
        List<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> Filter(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, object>>> includes);
        void Add(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void  Delete(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entityToUpdate);
        IEnumerable<TEntity> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending = true);
        IEnumerable<TEntity> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending = true, string includeProperties = "");
        IEnumerable<TEntity> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters);
        IEnumerable<TEntity> GetFromDatabaseWithQuery(string sqlQuery);
        int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters);
        int ExecuteInDatabaseByQuery(string sqlCommand);
        int SaveChanges();

        /*** Asincronos ***/
        Task<TEntity> GetByIDAsync(object id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(List<Expression<Func<TEntity, object>>> includes);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, object>>> includes);
        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, object>>> includes);
        Task<int> ExecuteInDatabaseByQueryAsync(string sqlCommand, params object[] parameters);
        DataTable GetDataTable(string sqlCommand, params object[] parameters);
        Task<int> ExecuteInDatabaseByQueryAsync(string sqlCommand);
        Task<int> SaveChangesAsync();
    }
}
