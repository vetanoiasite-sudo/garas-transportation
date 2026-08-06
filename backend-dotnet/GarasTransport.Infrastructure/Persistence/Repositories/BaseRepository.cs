using System.Linq.Expressions;
using GarasTransport.Domain.Constants;
using GarasTransport.Domain.Helper;
using GarasTransport.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Infrastructure.Persistence.Repositories;

/// <summary>Generic EF Core repository (doc §9.1) — the only classes touching the DbContext.</summary>
public class BaseRepository<T, DT> : IBaseRepository<T, DT> where T : class
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context) => _context = context;

    public T? GetById(DT id) => _context.Set<T>().Find(id);

    public async Task<T?> GetByIdAsync(DT id) => await _context.Set<T>().FindAsync(id);

    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        return await query.FirstOrDefaultAsync(criteria);
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? criteria = null, string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        if (criteria != null) query = query.Where(criteria);
        return await query.ToListAsync();
    }

    public IQueryable<T> FindAllQueryable(Expression<Func<T, bool>>? criteria = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (criteria != null) query = query.Where(criteria);
        return query;
    }

    public async Task<PagedList<T>> FindAllPagingAsync(
        Expression<Func<T, bool>>? criteria,
        int page,
        int pageSize,
        string[]? includes = null,
        Expression<Func<T, object>>? orderBy = null,
        string orderByDirection = OrderBy.Descending)
    {
        IQueryable<T> query = _context.Set<T>();
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        if (criteria != null) query = query.Where(criteria);

        var total = await query.CountAsync();

        if (orderBy != null)
            query = orderByDirection == OrderBy.Ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

        var items = await query
            .Skip((Math.Max(1, page) - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<T>
        {
            Items = items,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = total,
        };
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(List<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

    public bool Any(Expression<Func<T, bool>> criteria) => _context.Set<T>().Any(criteria);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> criteria) => _context.Set<T>().AnyAsync(criteria);

    public int Count(Expression<Func<T, bool>>? criteria = null) =>
        criteria == null ? _context.Set<T>().Count() : _context.Set<T>().Count(criteria);

    public Task<int> CountAsync(Expression<Func<T, bool>>? criteria = null) =>
        criteria == null ? _context.Set<T>().CountAsync() : _context.Set<T>().CountAsync(criteria);
}
