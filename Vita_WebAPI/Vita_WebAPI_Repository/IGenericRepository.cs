namespace Vita_WebAPI_Repository;

public interface IGenericRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(int id);
    public Task<List<T>> GetAllAsync();
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task<T> DeleteAsync(int id);
}