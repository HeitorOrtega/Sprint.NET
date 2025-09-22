using Sprint_1.Models;
using Sprint_1.Helpers;

public interface IPatioService
{
    Task<(IEnumerable<Patio> Items, int TotalCount)> GetAllAsync(QueryParameters parameters);
    Task<Patio?> GetByIdAsync(long id);
    Task<Patio> CreateAsync(Patio p);
    Task<Patio?> UpdateAsync(long id, Patio p);
    Task<bool> DeleteAsync(long id);
}