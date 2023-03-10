using System.ComponentModel.DataAnnotations;

namespace Common.Entities;

/// <summary>
/// An interface for mark entity classes
/// </summary>
public interface IEntity
{
}

/// <summary>
/// An interface for mark entity classes
/// </summary>
/// <typeparam name="TKey">Id key type</typeparam>
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}

public interface ITimeModification
{
    DateTime CreatedTime { get; set; }
    DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// Base entity class with Id key
/// </summary>
/// <typeparam name="TKey">Id key type</typeparam>
public abstract class BaseEntity<TKey> : IEntity<TKey>, ITimeModification
{
    [Key]
    public TKey Id { get; set; }

    public DateTime CreatedTime { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public override bool Equals(object obj)
    {
        if (!(obj is BaseEntity<TKey> other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(BaseEntity<TKey> a, BaseEntity<TKey> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TKey> a, BaseEntity<TKey> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}

/// <summary>
/// Base entity class with integer Id key
/// </summary>
public abstract class BaseEntity : BaseEntity<int>
{
}