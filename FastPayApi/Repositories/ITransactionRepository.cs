using FastPayApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastPayApi.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction tx);
        Task<List<Transaction>> GetByUserAsync(int userId);
        Task<List<Transaction>> GetAllAsync();
    }
}