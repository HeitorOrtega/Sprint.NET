using Microsoft.EntityFrameworkCore;
using Sprint.Data;
using Sprint.Helpers;
using Sprint.Models;
using Oracle.ManagedDataAccess.Client; 
using System.Data;

public class FuncionarioService : IFuncionarioService
{
    private readonly AppDbContext _db;
    
    public FuncionarioService(AppDbContext db) => _db = db;

    public async Task<(IEnumerable<Funcionario> Items, int TotalCount)> GetAllAsync(QueryParameters parameters)
    {
        var query = _db.Funcionarios.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(f => f.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Funcionario?> GetByIdAsync(long id) => await _db.Funcionarios.FindAsync(id);

    public async Task<Funcionario> CreateAsync(Funcionario f)
    {
        _db.Funcionarios.Add(f);
        await _db.SaveChangesAsync();
        return f;
    }

    public async Task<Funcionario?> UpdateAsync(long id, Funcionario f)
    {
        var existing = await _db.Funcionarios.FindAsync(id);
        if (existing == null) return null;

        existing.Nome = f.Nome;
        existing.Cpf = f.Cpf;
        existing.Email = f.Email;
        existing.Rg = f.Rg;
        existing.Telefone = f.Telefone;
        existing.PatioId = f.PatioId; 

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existing = await _db.Funcionarios.FindAsync(id);
        if (existing == null) return false;
        _db.Funcionarios.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<string> MoverFuncionarioParaPatioAsync(string cpfFuncionario, int novoPatioId)
    {
        var connection = (OracleConnection)_db.Database.GetDbConnection();
        string procedureName = "PKG_GESTAO_PATIO.PRC_ATUALIZAR_FUNCIONARIO_PATIO";

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        using (OracleCommand command = new OracleCommand(procedureName, connection))
        {
            try
            {
                command.CommandType = CommandType.StoredProcedure; 

                command.Parameters.Add("p_cpf_funcionario", OracleDbType.Varchar2, cpfFuncionario, ParameterDirection.Input);
                command.Parameters.Add("p_novo_patio_id", OracleDbType.Int32, novoPatioId, ParameterDirection.Input);
                
                await command.ExecuteNonQueryAsync();
                
                return $"✅ SUCESSO! Funcionário {cpfFuncionario} movido para o Pátio {novoPatioId}.";
            }
            catch (OracleException ex)
            {
                if (ex.Number >= 20001 && ex.Number <= 20999)
                {
                    return $"❌ ERRO DE NEGÓCIO ({ex.Number}): {ex.Message.Replace("\n", "").Trim()}";
                }
                return $"❌ ERRO INESPERADO ({ex.Number}): {ex.Message}";
            }
        }
    }
}
