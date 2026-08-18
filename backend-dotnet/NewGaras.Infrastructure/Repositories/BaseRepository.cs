using Microsoft.EntityFrameworkCore;
using NewGaras.Domain.Consts;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Helper;
using System.Linq.Expressions;


namespace NewGaras.Infrastructure.Repositories
{
    public class BaseRepository<T, DT> : IBaseRepository<T, DT> where T : class
    {
        protected GarasTestContext _context;
        public BaseRepository(GarasTestContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(DT id)
        {
            return _context.Set<T>().Find(id);
        }

        public T GetByIdAsNoTracking(DT id)
        {
            // Assuming your entity has an Id property
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "Id");
            var constant = Expression.Constant(id);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return _context.Set<T>().AsNoTracking().FirstOrDefault(lambda);
        }


        public DateTime? GetMaxDateTime(Expression<Func<T, DateTime>> criteria)
        {
            var query = _context.Set<T>().Select(criteria);
            return query.Any() ? query.Max() : null;
        }
        public long GetMaxLong(Expression<Func<T, long>> criteria)
        {
           var query = _context.Set<T>().Select(criteria);
            return query.Any() ? query.Max() : 0;
        }

        public int GetMaxInt(Expression<Func<T, int>> criteria)
        {
            var query = _context.Set<T>().Select(criteria);
            return query.Any() ? query.Max() : 0;
        }

        public decimal GetMaxDecimal(Expression<Func<T, decimal>> criteria)
        {
            var query = _context.Set<T>().Select(criteria);
            return query.Any() ? query.Max() : 0;
        }

        public async Task<T> GetByIdAsync(DT id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.FirstOrDefault(criteria);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.FirstOrDefaultAsync(criteria);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }
        public IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).AsQueryable();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (orderBy != null)
            {
                if (orderByDirection == ApplicationConsts.OrderByAscending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }
        public PagedList<T> FindAllPaging(Expression<Func<T, bool>> criteria, int? CurrentPage, int? NumberOfItemsPerPage
    , string[] includes = null,
    Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending)
        {

            IQueryable<T> query = _context.Set<T>().Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);


            int CP = 1;
            int TP = 100;
            if (CurrentPage != null)
                CP = (int)CurrentPage;

            if (NumberOfItemsPerPage != null)
                TP = (int)NumberOfItemsPerPage;
            if (orderBy != null)
            {
                if (orderByDirection == ApplicationConsts.OrderByAscending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            var PagingList = PagedList<T>.Create(query, CP, TP);
            return PagingList;
        }
        public async Task<PagedList<T>> FindAllPagingAsync(Expression<Func<T , bool>> criteria , int? CurrentPage , int? NumberOfItemsPerPage
, string [] includes = null ,
Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending)
        {

            IQueryable<T> query = _context.Set<T>().Where(criteria);
            if(includes!=null)
                foreach(var include in includes)
                    query=query.Include(include);


            int CP = 1;
            int TP = 100;
            if(CurrentPage!=null)
                CP=(int)CurrentPage;

            if(NumberOfItemsPerPage!=null)
                TP=(int)NumberOfItemsPerPage;
            query=query.OrderBy(orderBy);
            if(orderBy!=null)
            {
                if(orderByDirection==ApplicationConsts.OrderByAscending)
                    query=query.OrderBy(orderBy);
                else
                    query=query.OrderByDescending(orderBy);
            }
            var PagingList = await PagedList<T>.CreateAsync(query, CP, TP);
            return PagingList;
        }
        public async Task<IEnumerable<TResult>> FindAllBySelectionAsync<TResult>(Expression<Func<T, bool>> ConditionCriteria, Expression<Func<T, TResult>> SelectionCriteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(ConditionCriteria).Select(SelectionCriteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip)
        {
            return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == ApplicationConsts.OrderByAscending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take, string[] includes = null,
    Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
            {
                if (orderByDirection == ApplicationConsts.OrderByAscending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);


            return await query.ToListAsync();
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            return entities;
        }
        
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _context.Set<T>().Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }
        public IQueryable<T> GetAsQueryable() 
        {
            return _context.Set<T>().AsQueryable();
        }


        public IQueryable<T> FindAllQueryablePaging(Expression<Func<T, bool>> criteria, int? CurrentPage, int? NumberOfItemsPerPage
    , string[] includes = null,
    Expression<Func<T, object>> orderBy = null, string orderByDirection = ApplicationConsts.OrderByAscending)
        {

            //IQueryable<T> query = _context.Set<T>();

            //if (includes != null)
            //    foreach (var include in includes)
            //        query = query.Include(include);

            //return query.Where(criteria).AsQueryable();


            IQueryable<T> query = _context.Set<T>().Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);


            int CP = 1;
            int TP = 100;
            if (CurrentPage != null)
                CP = (int)CurrentPage;

            if (NumberOfItemsPerPage != null)
                TP = (int)NumberOfItemsPerPage;
            if (orderBy != null)
            {
                if (orderByDirection == ApplicationConsts.OrderByAscending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            var PagingList = PagedList<T>.Create(query, CP, TP).AsQueryable();
            return PagingList;
        }


        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        // If you're using async operations, you can use:
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }


    }

     
}
