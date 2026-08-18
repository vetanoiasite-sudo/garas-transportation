using NewGaras.Domain.Consts;
using NewGaras.Infrastructure.Helper;
using System.Linq.Expressions;


namespace NewGaras.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T, DT> where T : class
    {
        T GetById(DT id);
        T GetByIdAsNoTracking(DT id);
        Task<T> GetByIdAsync(DT id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<TResult>> FindAllBySelectionAsync<TResult>(Expression<Func<T, bool>> ConditionCriteria, Expression<Func<T, TResult>> SelectionCriteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take, string[] includes = null,
    Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);

        public long GetMaxLong(Expression<Func<T, long>> criteria);
        public DateTime? GetMaxDateTime(Expression<Func<T, DateTime>> criteria);
        public int GetMaxInt(Expression<Func<T, int>> criteria);
        public decimal GetMaxDecimal(Expression<Func<T, decimal>> criteria);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        public IQueryable<T> GetAsQueryable();
        public PagedList<T> FindAllPaging(Expression<Func<T, bool>> criteria, int? CurrentPage, int? NumberOfItemsPerPage
    , string[] includes = null,
    Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending);

        Task<PagedList<T>> FindAllPagingAsync(Expression<Func<T , bool>> criteria , int? CurrentPage , int? NumberOfItemsPerPage
    , string [] includes = null ,
    Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending);
        public IQueryable<T> FindAllQueryablePaging(Expression<Func<T, bool>> criteria, int? CurrentPage, int? NumberOfItemsPerPage
    , string[] includes = null, Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending);

        public  Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        public bool Any(Expression<Func<T, bool>> predicate);
    }
}
