using System.Collections.Concurrent;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public abstract class InMemoryRepository<T> : IRepository<T> where T : class
{
    protected readonly ConcurrentDictionary<int, T> _entities = new();
    private readonly object _idLock = new();
    
    protected abstract int GetEntityId(T entity);
    protected abstract void SetEntityId(T entity, int id);

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        _entities.TryGetValue(id, out var entity);
        return await Task.FromResult(entity);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Task.FromResult(_entities.Values.ToList());
    }

    public virtual async Task AddAsync(T entity)
    {
        // Generate ID in a thread-safe manner
        int id;
        lock (_idLock)
        {
            id = _entities.Count > 0 ? _entities.Keys.Max() + 1 : 1;
        }

        SetEntityId(entity, id);
        
        if (!_entities.TryAdd(id, entity))
        {
            throw new ValidationException($"Failed to add {typeof(T).Name}. An entity with ID {id} already exists.");
        }
        
        await Task.CompletedTask;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var id = GetEntityId(entity);
        
        if (!_entities.ContainsKey(id))
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        if (!_entities.TryUpdate(id, entity, _entities[id]))
        {
            throw new ValidationException($"Failed to update {typeof(T).Name} with ID {id}. The entity may have been modified.");
        }
        
        await Task.CompletedTask;
    }   

    public virtual async Task DeleteAsync(T entity)
    {
        var id = GetEntityId(entity);
        
        if (!_entities.ContainsKey(id))
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        if (!_entities.TryRemove(id, out _))
        {
            throw new ValidationException($"Failed to delete {typeof(T).Name} with ID {id}. The entity may have been modified.");
        }
        
        await Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await Task.FromResult(_entities.ContainsKey(id));
    }
}