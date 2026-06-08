using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class //cip...65
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper; //cip...65

    public GenericRepository(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        this._mapper = mapper;
    }

    public async Task AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        //or await _authorsRepository.Set<T>().AddAsync(entity); but not needed as the set type can be inferred by entity
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Exists(int id)
    {
        return await GetAsync(id) != null;
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    //20260608 chatgpt potential fix for azure only showing 106 items in azure
    //public async Task<PagedResult<TResult>> GetAsync<TResult>(QueryParameters queryParams) where TResult : class //cip...66
    //{
    //    var totalCount = await _context.Set<T>().CountAsync();
    //    var items = await _context.Set<T>()
    //      .Skip((queryParams.StartIndex))
    //      .Take(queryParams.PageSize)
    //      .ProjectTo<TResult>(_mapper.ConfigurationProvider)
    //      .ToListAsync();

    //    return new PagedResult<TResult> { TotalCount = totalCount, Items = items };
    //}

    public async Task<PagedResult<TResult>> GetAsync<TResult>(QueryParameters queryParams) //20260608 chatgpt potential fix for azure only showing 106 items in azure. also, introduced sorting and searching parameters for future use.
        where TResult : class
    {
        var baseQuery = BuildBaseQuery();

        var totalSize = await baseQuery.CountAsync();

        var items = await ApplyOrdering(baseQuery, queryParams)
            .Skip(queryParams.StartIndex)
            .Take(queryParams.PageSize)
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TResult>
        {
            TotalCount = totalSize,
            Items = items
        };
    }

    private IQueryable<T> BuildBaseQuery() //20260608 chatgpt potential fix for azure only showing 106 items in azure
    {
        var query = _context.Set<T>().AsQueryable();

        // Example: soft delete support (if you have it)
        // query = query.Where(x => !x.IsDeleted);

        // Example: search hook (optional)
        // query = ApplySearch(query, queryParams.Search);

        return query;
    }

    private static IQueryable<T> ApplyOrdering(IQueryable<T> query, QueryParameters p) //20260608 chatgpt potential fix for azure only showing 106 items in azure. also, introduced sorting and searching parameters for future use.
    {
        if (!string.IsNullOrWhiteSpace(p.SortBy))
        {
            var sortDesc = p.SortDesc ?? false; //cip...20260609 chatgpt fix: handle nullability of SortDesc
            return sortDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, p.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, p.SortBy));
        }

        // fallback deterministic order
        return query.OrderBy(e => EF.Property<object>(e, "Id"));
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
