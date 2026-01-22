using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.Services
{
    public interface IDataManipulationService
    {
        Task<PersistentBase> SaveAsync(PersistentBase item, CancellationToken ct = default);
        Task DeleteAsync(PersistentBase item, CancellationToken ct = default);
    }

    public class DataManipulationService<TEntity>(IServiceProvider sp, EntityContext entityContext) : IDataManipulationService where TEntity : PersistentBase
    {
        public async Task<PersistentBase> SaveAsync(PersistentBase item, CancellationToken ct = default)
        {
            if(item.CreationTime == DateTime.MinValue)
            {
                return await SaveNew((TEntity)item, ct);
            }

            return await UpdateExisting((TEntity)item, ct);
        }

        private async Task<TEntity> SaveNew(TEntity item, CancellationToken ct = default)
        {
            item = await SetNavigations(item, ct);
            TrackGraphForInsert(item);

            await entityContext.SaveChangesAsync(ct);
            return item;
        }

        private async Task<TEntity> UpdateExisting(TEntity item, CancellationToken ct)
        {
            TEntity trackedEntity = await GetExisting(item.Id, ct);

            entityContext.Entry(trackedEntity).CurrentValues.SetValues(item);

            IGraphMerger<TEntity>? merger = sp.GetService<IGraphMerger<TEntity>>();
            if (merger != null)
            {
                await merger.Merge(entityContext, trackedEntity, item, ct);
            }

            await entityContext.SaveChangesAsync(ct);
            return trackedEntity;
        }

        private async Task<TEntity> GetExisting(Guid itemId, CancellationToken ct)
        {
            IQueryable<TEntity> q = entityContext.Set<TEntity>();

            IQueryProfile<TEntity> queryProfile = sp.GetRequiredService<IQueryProfile<TEntity>>();
            q = queryProfile.Apply(q);

            return await q.SingleAsync(x => x.Id == itemId, ct);
        }

        private async Task<TEntity> SetNavigations(TEntity savingEntity, CancellationToken ct = default)
        {
            if (savingEntity is Order order)
            {
                Customer? customer = await entityContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == order.CustomerId, ct);
                if (customer != null)
                {
                    order.Customer = customer;
                }
            }

            return savingEntity;
        }

        private void TrackGraphForInsert(TEntity rootEntity)
        {
            entityContext.ChangeTracker.TrackGraph(rootEntity, node =>
            {
                PersistentBase entity = (PersistentBase)node.Entry.Entity;
                node.Entry.State = entity.CreationTime == DateTime.MinValue ? EntityState.Added : EntityState.Unchanged;
            });
        }



        public async Task DeleteAsync(PersistentBase? item, CancellationToken ct = default)
        {
            if (item is null)
            {
                return;
            }

            TEntity? trackedEntity = await entityContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == item.Id, ct);
            if (trackedEntity is null)
            {
                return;
            }

            entityContext.Remove(trackedEntity);
            await entityContext.SaveChangesAsync(ct);
            throw new System.NotImplementedException();
        }
    } 
}
