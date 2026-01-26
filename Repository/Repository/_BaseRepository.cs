using Domain.Data.Entities._Base;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Constant = Domain.Constants.Constants;

namespace Repository.Repository;

public abstract class BaseRepository<E> where E : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<E> _entity;

    protected BaseRepository(AppDbContext dbContext, DbSet<E> dbEntity)
    {
        _context = dbContext;
        _entity = dbEntity;
    }

    public async Task<E?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        E? response;

        try
        {
            response = await _entity
                .Where(x => x.Id == id && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<E> InsertAsync(E entity, string actorId = null)
    {
        try
        {
            entity.Id = entity.Id == null ? Guid.NewGuid().ToString() : entity.Id;
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.CreatedBy = actorId ?? Constant.Settings.SystemId;
            entity.UpdatedBy = actorId ?? Constant.Settings.SystemId;

            await _entity.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return entity;
    }

    public async Task<E> UpdateAsync(E entry, string actorId = null)
    {
        try
        {
            var dbentry = await _entity.Where(e => e.Id == entry.Id).FirstOrDefaultAsync();

            _context.Entry(dbentry).CurrentValues.SetValues(entry);

            // Avoid changing auditable fields
            _context.Entry(dbentry).Property(x => x.CreatedAt).IsModified = false;
            _context.Entry(dbentry).Property(x => x.CreatedBy).IsModified = false;
            _context.Entry(dbentry).Property(x => x.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
            _context.Entry(dbentry).Property(x => x.UpdatedBy).CurrentValue = actorId ?? Constant.Settings.SystemId;

            // Send to database
            await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return entry;
    }

    public async Task<E> DeleteAsync(E entity, string actorId = null)
    {
        try
        {
            // Avoid changing auditable fields
            _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            _context.Entry(entity).Property(x => x.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
            _context.Entry(entity).Property(x => x.DeletedAt).CurrentValue = DateTimeOffset.UtcNow;
            _context.Entry(entity).Property(x => x.UpdatedBy).CurrentValue = actorId ?? Constant.Settings.SystemId;

            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return entity;
    }

    public async Task HardDeleteAsync(E entity)
    {
        try
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<IEnumerable<E>> GetByIdListAsync(IEnumerable<String> idList, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => idList.Contains(x.Id) && x.DeletedAt == null)
                .AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<IEnumerable<E>> InsertListAsync(IEnumerable<E> entityList, string actorId = null)
    {
        try
        {
            foreach (E entity in entityList)
            {
                entity.Id = Guid.NewGuid().ToString();
                entity.CreatedAt = DateTimeOffset.UtcNow;
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                entity.CreatedBy = actorId ?? Constant.Settings.SystemId;
                entity.UpdatedBy = actorId ?? Constant.Settings.SystemId;
            }

            await _context.AddRangeAsync(entityList);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return entityList;
    }

    public async Task<IEnumerable<E>> UpdateListAsync(IEnumerable<E> entityList, string actorId = null)
    {
        try
        {
            List<String> idStringList = entityList.Select(m => m.Id).ToList();

            IList<E> existingList = await _entity
                .Where(x => idStringList.Contains(x.Id)
                            && x.DeletedAt == null)
                .ToListAsync();

            DateTimeOffset createdAt;
            string createdBy;
            E matchingEntity;

            foreach (E existing in existingList)
            {
                matchingEntity = entityList.Where(ex => ex.Id == existing.Id).FirstOrDefault();

                if (matchingEntity != null)
                {
                    createdAt = existing.CreatedAt;
                    createdBy = existing.CreatedBy;

                    _context.Entry(existing).CurrentValues.SetValues(matchingEntity);

                    existing.CreatedAt = createdAt;
                    existing.CreatedBy = createdBy;
                    existing.UpdatedAt = DateTimeOffset.UtcNow;
                    existing.UpdatedBy = actorId ?? Constant.Settings.SystemId;
                }
            }

            await _context.SaveChangesAsync();

            return existingList;
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<IEnumerable<E>> DeleteListAsync(IEnumerable<String> idList, string actorId = null)
    {
        try
        {
            IList<E> existingList = await _entity
                .Where(x => idList.Contains(x.Id)
                            && x.DeletedAt == null)
                .ToListAsync();

            foreach (E existing in existingList)
            {
                _context.Entry(existing).Property(x => x.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
                _context.Entry(existing).Property(x => x.DeletedAt).CurrentValue = DateTimeOffset.UtcNow;
                _context.Entry(existing).Property(x => x.UpdatedBy).CurrentValue =
                    actorId ?? Constant.Settings.SystemId;
            }

            await _context.SaveChangesAsync();
            return existingList;
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<IEnumerable<E>> HardDeleteListAsync(IEnumerable<String> idList)
    {
        IEnumerable<E> response;

        try
        {
            response = await _entity.Where(x => idList.Contains(x.Id)).ToListAsync();
            _context.RemoveRange(response);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<IEnumerable<E>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<E> response;

        try
        {
            response = await _entity
                .Where(e => e.DeletedAt == null)
                .AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public IQueryable<E> GetByExpression(Expression<Func<E, bool>> expression)
    {
        IQueryable<E> response;

        try
        {
            response = _entity.Where(expression)
                .Where(e => e.DeletedAt == null)
                .AsNoTracking();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public IQueryable<E> GetDeletionByExpression(Expression<Func<E, bool>> expression)
    {
        IQueryable<E> response;

        try
        {
            response = _entity.Where(expression)
                .Where(e => e.DeletedAt.HasValue)
                .AsNoTracking();
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<T> GetMaxAsync<T>(Expression<Func<E, bool>> query, Expression<Func<E, T>> selector, CancellationToken cancellationToken = default)
    {
        T response;

        try
        {
            response = await _entity
                .Where(query)
                .Where(x => x.DeletedAt == null)
                .MaxAsync(selector, cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<T> GetMaxAsync<T>(Expression<Func<E, T>> selector, CancellationToken cancellationToken = default)
    {
        T response;

        try
        {
            response = await _entity
                .Where(x => x.DeletedAt == null)
                .MaxAsync(selector, cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<T> GetMinAsync<T>(Expression<Func<E, bool>> query, Expression<Func<E, T>> selector, CancellationToken cancellationToken = default)
    {
        T response;

        try
        {
            response = await _entity
                .Where(query)
                .Where(x => x.DeletedAt == null)
                .MinAsync(selector, cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<T> GetMinAsync<T>(Expression<Func<E, T>> selector, CancellationToken cancellationToken = default)
    {
        T response;

        try
        {
            response = await _entity
                .Where(x => x.DeletedAt == null)
                .MinAsync(selector, cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<Boolean> Exists(Expression<Func<E, bool>> expression, CancellationToken cancellationToken = default)
    {
        Boolean response;

        try
        {
            response = await _entity.Where(expression).AsNoTracking().AnyAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<Int32> CountAll(CancellationToken cancellationToken = default)
    {
        Int32 response;

        try
        {
            response = await _entity.CountAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<Int32> CountWhere(Expression<Func<E, bool>> expression, CancellationToken cancellationToken = default)
    {
        Int32 response;

        try
        {
            response = await _entity.CountAsync(expression, cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<E> UpdatePartialAsync(E entry, Action<E> updateAction, string actorId = null)
    {
        try
        {
            var dbEntry = await _entity
                                .Where(e => e.Id == entry.Id)
                                .FirstOrDefaultAsync() ?? throw new PersistenceException($"Entity with ID {entry.Id} not found.");

            updateAction(dbEntry);

            _context.Entry(dbEntry).Property(x => x.CreatedAt).IsModified = false;
            _context.Entry(dbEntry).Property(x => x.CreatedBy).IsModified = false;
            _context.Entry(dbEntry).Property(x => x.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
            _context.Entry(dbEntry).Property(x => x.UpdatedBy).CurrentValue = actorId ?? Constant.Settings.SystemId;

            await _context.SaveChangesAsync();

            return dbEntry;
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }
    }
}
