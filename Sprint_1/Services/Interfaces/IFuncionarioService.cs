using Sprint_1.Models;
using Sprint_1.Helpers;

public interface IFuncionarioService
{
    Task<(IEnumerable<Funcionario> Items, int TotalCount)> GetAllAsync(QueryParameters parameters);
    Task<Funcionario?> GetByIdAsync(long id);
    Task<Funcionario> CreateAsync(Funcionario f);
    Task<Funcionario?> UpdateAsync(long id, Funcionario f);
    Task<bool> DeleteAsync(long id);
}