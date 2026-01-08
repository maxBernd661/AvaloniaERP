namespace AvaloniaERP.Core.Entity
{
    public class PersistentBase
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
