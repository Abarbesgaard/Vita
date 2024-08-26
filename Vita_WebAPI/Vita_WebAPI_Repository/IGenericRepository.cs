namespace Vita_WebAPI_Repository;

public interface IGenericRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(int id);
    public Task<List<T>> GetAllAsync();
    public Task AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
}