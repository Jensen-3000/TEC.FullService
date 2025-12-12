namespace TEC.FullService.Api.Domain.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    Guid? DeletedBy { get; }

    void Delete(Guid? userId = null);
    void Restore();
}

public abstract class SoftDeletableEntity : AuditableEntity, ISoftDeletable
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }

    public virtual void Delete(Guid? userId = null)
    {
        if (IsDeleted)
            throw new DomainException("Entity is already deleted.");

        IsDeleted = true;
        if (userId is not null)
            DeletedBy = userId;
    }

    public virtual void Restore()
    {
        if (!IsDeleted)
            throw new DomainException("Entity is not deleted.");

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}
