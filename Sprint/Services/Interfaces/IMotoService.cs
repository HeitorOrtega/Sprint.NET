using Sprint.Models;
using Sprint.Helpers;

public interface IMotoService
{
    Task<(IEnumerable<Moto> Items, int TotalCount)> GetAllAsync(QueryParameters parameters);
    Task<Moto?> GetByIdAsync(long id);
    Task<Moto> CreateAsync(Moto m);
    Task<Moto?> UpdateAsync(long id, Moto m);
    Task<bool> DeleteAsync(long id);
}