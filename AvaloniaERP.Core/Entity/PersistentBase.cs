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
    }

    public abstract class RowBase<TEntity> : IEntityRow where TEntity : PersistentBase
    {
        public Type EntityType = typeof(TEntity);


        public abstract string AsString();

        public string DisplayString => AsString();
    }
}
