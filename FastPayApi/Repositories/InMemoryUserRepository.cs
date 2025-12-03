using FastPayApi.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastPayApi.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<int, User> _store = new();
        private int _id = 0;

        public InMemoryUserRepository()
        {
            // seed a couple of users
            AddAsync(new User { Name = "Adonis Sena", Phone = "+5959000001", Email = "adonis@example.com", Balance = 100m, Pin = "1234" }).Wait();
            AddAsync(new User { Name = "María Pérez", Phone = "+5959000002", Email = "maria@example.com", Balance = 50m, Pin = "4321" }).Wait();
        }

        public Task<User?> GetByIdAsync(int id)
        {
            _store.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<User?> GetByPhoneAsync(string phone)
        {
            var user = _store.Values.FirstOrDefault(u => u.Phone == phone);
            return Task.FromResult(user);
        }

        public Task<List<User>> GetAllAsync()
        {
            return Task.FromResult(_store.Values.OrderBy(u => u.Id).ToList());
        }

        public Task<User> AddAsync(User user)
        {
            user.Id = System.Threading.Interlocked.Increment(ref _id);
            _store[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task UpdateAsync(User user)
        {
            _store[user.Id] = user;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            _store.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}