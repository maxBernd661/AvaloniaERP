namespace AvaloniaERP.Core.Entity
{
    public class PersistentBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    public interface IEntityRow
    {
        string AsString();

        Guid Id { get; }
    }

    public abstract class RowBase<TEntity> : IEntityRow where TEntity : PersistentBase
    {
        public Type EntityType = typeof(TEntity);

        public abstract string AsString();

        public abstract Guid Id { get; }

        public string DisplayString
        {
            get { return AsString(); }
        }
    }

    public interface IQueryProfile<TEntity> where TEntity : PersistentBase
    {
        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }

    public interface IGraphMerger<in TEntity> where TEntity : PersistentBase
    {
        Task Merge(EntityContext context, TEntity tracked, TEntity incoming, CancellationToken ct = default);
    }
}