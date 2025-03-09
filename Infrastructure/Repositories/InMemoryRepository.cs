using System.Collections.Concurrent;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public abstract class InMemoryRepository<T> : IRepository<T> where T : class
{
    protected readonly ConcurrentDictionary<int, T> _entities = new();
    private int _lastId;

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            _entities.TryGetValue(id, out var entity);
            return entity;
        }, ct);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.ToList();
        }, ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var id = Interlocked.Increment(ref _lastId);
            SetEntityId(entity, id);
            if (!_entities.TryAdd(id, entity))
            {
                throw new InvalidOperationException($"Failed to add entity with ID {id}");
            }
        }, ct);
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var id = GetEntityId(entity);
            if (!_entities.ContainsKey(id))
            {
                throw new InvalidOperationException($"Entity with ID {id} does not exist.");
            }
            if (!_entities.TryUpdate(id, entity, _entities[id]))
            {
                throw new InvalidOperationException($"Failed to update entity with ID {id}");
            }
        }, ct);
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var id = GetEntityId(entity);
            if (!_entities.ContainsKey(id))
            {
                throw new InvalidOperationException($"Entity with ID {id} does not exist.");
            }
            if (!_entities.TryRemove(id, out _))
            {
                throw new InvalidOperationException($"Failed to delete entity with ID {id}");
            }
        }, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.ContainsKey(id);
        }, ct);
    }

    protected abstract int GetEntityId(T entity);
    protected abstract void SetEntityId(T entity, int id);
}