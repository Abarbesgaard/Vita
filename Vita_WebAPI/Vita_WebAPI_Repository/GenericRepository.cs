using Microsoft.EntityFrameworkCore;
using Vita_WebAPI_Data;

namespace Vita_WebAPI_Repository;

public class GenericRepository<T>(DataContext context) : IGenericRepository<T> where T : class
{
    public async Task DeleteAsync(T entity)
    {
        context.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await context.Set<T>()
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task AddAsync(T entity)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.FindAsync<T>(id);
    }

    public async Task UpdateAsync(T entity)
    {
        context.Update(entity);
        await context.SaveChangesAsync();
    }
}