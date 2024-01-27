using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NMoneys.Serialization.EFCore;

/// <summary>
/// Specifies custom value snapshotting and comparison for <see cref="Money"/>.
/// </summary>
public class MoneyComparer : ValueComparer<Money>
{
	/// <summary>
	/// Creates a new <see cref="MoneyComparer"/>. A shallow copy will be used for the snapshot.
	/// </summary>
	public MoneyComparer() : base((x, y) => x.Equals(y), m => m.GetHashCode()) { }
}
