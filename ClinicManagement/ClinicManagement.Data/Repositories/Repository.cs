using Dapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ClinicManagement.Data.Connection;
using ClinicManagement.Data.Interface;
using System.Data;
using System.Linq.Expressions;

namespace ClinicManagement.Data.Repositories
{
    public class Repository<T>(IUnitOfWork unitOfWork) : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly DatabaseContext _context = unitOfWork.DbContext;

        public void Add(T entity)
        {
            _context.Entry(entity);
            _context.Set<T>().Add(entity);
        }

        public void AddRange(List<T> entity)
        {
            _context.Set<T>().AddRange(entity);
        }

        public virtual void BulkDelete(IList<T> items) => _context.BulkDelete(items);

        public virtual void BulkInsert(IList<T> items) => _context.BulkInsert(items);

        public async Task<bool> BulkInsertUsingDapper(string storedProcedure, object parms)
        {
            await _unitOfWork.Connection.ExecuteAsync(storedProcedure, parms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }

        public virtual void BulkUpdate(IList<T> items) => _context.BulkUpdate(items);

        public bool Commit()
        {
            var recordsCommittedCount = _context.SaveChanges();
            return (recordsCommittedCount > 0);
        }

        public async Task<bool> CommitAsync(string typeSave)
        {
            var recordsCommittedCount = await _context.SaveChangesAsync(typeSave);
            return (recordsCommittedCount > 0);
        }

        public int Count() => _context.Set<T>().Count();

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().CountAsync(predicate);

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public async Task<bool> DeleteUsingDapper(long id, string storedProcedure)
        {
            DynamicParameters p = new();
            p.Add("@Id", id);
            await _unitOfWork.Connection.ExecuteAsync(storedProcedure, p, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>();
            return query.AsQueryable();
        }

        public IQueryable<T> GetAllNoneDeleted()
        {
            IQueryable<T> query = _context.Set<T>();
            return query.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllUsingDapper(string storedProcedure) => await _unitOfWork.Connection.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);

        public async Task<T> GetByIdUsingDapper(long id, string storedProcedure)
        {
            DynamicParameters p = new();
            p.Add("@Id", id);
            var entity = await _unitOfWork.Connection.QuerySingleOrDefaultAsync<T>(storedProcedure, p, commandType: CommandType.StoredProcedure);
            return entity;
        }

#pragma warning disable CS8603 // Possible null reference return.
        public T GetSingle(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);
#pragma warning restore CS8603 // Possible null reference return.

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return query.FirstOrDefault(predicate);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return await query.FirstOrDefaultAsync(predicate);
#pragma warning restore CS8603 // Possible null reference return.
        }

#pragma warning disable CS8603 // Possible null reference return.
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().FirstOrDefaultAsync(predicate);
#pragma warning restore CS8603 // Possible null reference return.

#pragma warning disable CS8603 // Possible null reference return.
        public T GetSingleNoneDeleted(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);
#pragma warning restore CS8603 // Possible null reference return.

#pragma warning disable CS8603 // Possible null reference return.
        public async Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().FirstOrDefaultAsync(predicate);
#pragma warning restore CS8603 // Possible null reference return.

        public async Task<bool> InsertUsingDapper(string storedProcedure, object parms)
        {
            await _unitOfWork.Connection.ExecuteAsync(storedProcedure, parms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }

        public void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public async Task<bool> UpdateUsingDapper(T obj, string storedProcedure, object parms)
        {
            await _unitOfWork.Connection.ExecuteAsync(storedProcedure, parms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
    }
}
