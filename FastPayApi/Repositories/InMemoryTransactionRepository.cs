using FastPayApi.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastPayApi.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly ConcurrentDictionary<int, Transaction> _store = new();
        private int _id = 0;

        public Task<Transaction> AddAsync(Transaction tx)
        {
            tx.Id = System.Threading.Interlocked.Increment(ref _id);
            _store[tx.Id] = tx;
            return Task.FromResult(tx);
        }

        public Task<List<Transaction>> GetByUserAsync(int userId)
        {
            var list = _store.Values.Where(t => t.FromUserId == userId || t.ToUserId == userId).OrderByDescending(t => t.Timestamp).ToList();
            return Task.FromResult(list);
        }

        public Task<List<Transaction>> GetAllAsync()
        {
            return Task.FromResult(_store.Values.OrderByDescending(t => t.Timestamp).ToList());
        }
    }
}