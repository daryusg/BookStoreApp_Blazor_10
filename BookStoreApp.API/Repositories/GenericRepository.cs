using BookStoreApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
  public class GenericRepository<T> : IGenericRepository<T> where T : class //cip...65
  {
    private readonly BookStoreDbContext _context;

    public GenericRepository(BookStoreDbContext context)
    {
      _context = context;
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

    public async Task<bool> UpdateAsync(T entity)
    {
      _context.Update(entity);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}
