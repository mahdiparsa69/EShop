namespace EShop.Domain.Models;

public abstract class BaseModel
{
    public Guid Id { get; set; }

    public uint xmin { get; set; }

    public long SeqId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public bool IsDeleted { get; set; }
}

