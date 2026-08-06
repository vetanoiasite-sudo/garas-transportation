using System.Linq.Expressions;
using GarasTransport.Domain.Constants;
using GarasTransport.Domain.Helper;

namespace GarasTransport.Domain.IRepository;

/// <summary>
/// Generic repository contract (doc §9.1).
/// T = entity type, DT = id type (int, string, long, Guid...).
/// </summary>
public interface IBaseRepository<T, DT> where T : class
{
    T? GetById(DT id);
    Task<T?> GetByIdAsync(DT id);

    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? criteria = null, string[]? includes = null);

    /// <summary>Deferred LINQ — callers compose Where/Include/OrderBy/projection then materialize.</summary>
    IQueryable<T> FindAllQueryable(Expression<Func<T, bool>>? criteria = null);

    Task<PagedList<T>> FindAllPagingAsync(
        Expression<Func<T, bool>>? criteria,
        int page,
        int pageSize,
        string[]? includes = null,
        Expression<Func<T, object>>? orderBy = null,
        string orderByDirection = OrderBy.Descending);

    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(List<T> entities);
    T Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);

    bool Any(Expression<Func<T, bool>> criteria);
    Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);
    int Count(Expression<Func<T, bool>>? criteria = null);
    Task<int> CountAsync(Expression<Func<T, bool>>? criteria = null);
}
