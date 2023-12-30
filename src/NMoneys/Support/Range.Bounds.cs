namespace NMoneys.Support;

internal interface IBound<T> : IEquatable<IBound<T>> where T : struct, IComparable<T>
{
	T Value { get; }

	#region representation

	string Lower();
	string Upper();

	#endregion

	#region value checking

	bool LessThan(T other);
	bool MoreThan(T other);

	#endregion

	#region argument assertion

	string ToAssertion();

	#endregion
}

internal readonly struct Closed<T> : IEquatable<Closed<T>>, IBound<T> where T : struct, IComparable<T>
{
	public T Value { get; }

	public Closed(T value)
	{
		Value = value;
	}

	public string Lower()
	{
		return "[" + Value;
	}

	public string Upper()
	{
		return Value + "]";
	}

	public bool LessThan(T other)
	{
		return Value.CompareTo(other) <= 0;
	}

	public bool MoreThan(T other)
	{
		return Value.CompareTo(other) >= 0;
	}

	public string ToAssertion()
	{
		return Value + " (inclusive)";
	}

	#region Equality

	public bool Equals(IBound<T>? other)
	{
		return other is Closed<T> closed && Equals(closed);
	}

	public bool Equals(Closed<T> other)
	{
		return Value.Equals(other.Value);
	}

	public override bool Equals(object? obj)
	{
		return obj is Closed<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	#endregion
}

internal readonly struct Open<T> : IEquatable<Open<T>>, IBound<T> where T : struct, IComparable<T>
{
	public T Value { get; }

	public Open(T value)
	{
		Value = value;
	}

	public string Lower()
	{
		return "(" + Value;
	}

	public string Upper()
	{
		return Value + ")";
	}

	public bool LessThan(T other)
	{
		return Value.CompareTo(other) < 0;
	}

	public bool MoreThan(T other)
	{
		return Value.CompareTo(other) > 0;
	}

	public string ToAssertion()
	{
		return Value + " (not inclusive)";
	}

	#region Equality

	public bool Equals(IBound<T>? other)
	{
		return other is Open<T> open && Equals(open);
	}

	public bool Equals(Open<T> other)
	{
		return Value.Equals(other.Value);
	}

	public override bool Equals(object? obj)
	{
		return obj is Open<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	#endregion
}


internal static class Bound
{
	public static IBound<T> Close<T>(this T value) where T : struct, IComparable<T>
	{
		return new Closed<T>(value);
	}

	public static IBound<T> Open<T>(this T value) where T : struct, IComparable<T>
	{
		return new Open<T>(value);
	}
}
