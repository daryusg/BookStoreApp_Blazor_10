namespace BookStoreApp.API.Repositories
{
  public interface IGenericRepository<T> where T : class //cip...65
  {
    Task<List<T>> GetAsync();
    Task<T?> GetAsync(int id);
    Task AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> Exists(int id);
  }
}