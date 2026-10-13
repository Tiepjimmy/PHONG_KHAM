using System.Linq.Expressions;

namespace ClinicManagement.Data.Interface
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllUsingDapper(string storedProcedure);
        public Task<T> GetByIdUsingDapper(long id, string storedProcedure);
        public Task<bool> InsertUsingDapper(string storedProcedure, object parms);
        public Task<bool> BulkInsertUsingDapper(string storedProcedure, object parms);
        public Task<bool> UpdateUsingDapper(T obj, string storedProcedure, object parms);
        public Task<bool> DeleteUsingDapper(long id, string storedProcedure);
        public IQueryable<T> GetAll();
        public IQueryable<T> GetAllNoneDeleted();
        public void AddRange(List<T> entity);
        public int Count();
        public Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        public Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate);
        public Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        public T GetSingleNoneDeleted(Expression<Func<T, bool>> predicate);
        public T GetSingle(Expression<Func<T, bool>> predicate);
        public Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public void DeleteWhere(Expression<Func<T, bool>> predicate);
        public bool Commit();
        public Task<bool> CommitAsync(string typeSave);
        public void BulkInsert(IList<T> items);
        public void BulkUpdate(IList<T> items);
        public void BulkDelete(IList<T> items);
    }
}
