using System.Collections.Concurrent;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public abstract class InMemoryRepository<T> : IRepository<T> where T : class
{
    protected readonly ConcurrentDictionary<int, T> _entities = new();
    protected abstract int GetEntityId(T entity);
    protected abstract void SetEntityId(T entity, int id);

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        _entities.TryGetValue(id, out var entity);
        return await Task.FromResult(entity);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Task.FromResult(_entities.Values);
    }

    public virtual async Task AddAsync(T entity)
    {
        var id = _entities.Count + 1;
        SetEntityId(entity, id);
        _entities.TryAdd(id, entity);
        await Task.CompletedTask;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var id = GetEntityId(entity);
        _entities.TryUpdate(id, entity, _entities[id]);
        await Task.CompletedTask;
    }   

    public virtual async Task DeleteAsync(T entity)
    {
        var id = GetEntityId(entity);
        _entities.TryRemove(id, out _);
        await Task.CompletedTask;
    }


    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await Task.FromResult(_entities.ContainsKey(id));
    }
}