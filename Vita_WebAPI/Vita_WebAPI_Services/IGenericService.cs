namespace Vita_WebAPI_Services;

public interface IGenericService<T> where T : class
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task? CreateAsync(T entity);
    Task UpdateAsync(Guid id, T entity);
    Task DeleteAsync(Guid id);
}