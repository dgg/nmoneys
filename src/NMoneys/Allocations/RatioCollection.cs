using System.Collections;
using NMoneys.Support;

namespace NMoneys.Allocations;

/// <summary>
/// Maintains a list of ratios suitable for use in an allocation when the
/// sum of all items is exactly equal to one (100%).
/// </summary>
public class RatioCollection : IEnumerable<Ratio>, IFormattable
{
	private readonly Ratio[] _ratios;

	#region Creation

	/// <summary>
	/// Initializes a new instance of <see cref="RatioCollection"/> with the specified ratio values.
	/// </summary>
	/// <remarks>This is a helper constructor due to the verbosity of <see cref="Ratio"/> construction.</remarks>
	/// <param name="ratioValues">A collection of ratio values.</param>
	/// <exception cref="ArgumentNullException"><paramref name="ratioValues"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="ratioValues"/> do not sum up one
	/// -or-
	/// any of the values does not fall in the range [0..1].
	/// .</exception>
	public RatioCollection(params decimal[] ratioValues) : this(toRatios(ratioValues))
	{
	}

	private static Ratio[] toRatios(decimal[] ratioValues)
	{
		ArgumentNullException.ThrowIfNull(ratioValues, nameof(ratioValues));

		return ratioValues.Select(r => new Ratio(r)).ToArray();
	}

	/// <summary>
	/// Initializes a new instance of <see cref="RatioCollection"/> with the specified <paramref name="ratios"/>.
	/// </summary>
	/// <param name="ratios">A collection of ratios</param>
	/// <exception cref="ArgumentNullException"><paramref name="ratios"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="ratios"/> do not sum up one.</exception>
	public RatioCollection(params Ratio[] ratios)
	{
		assertAllocatable(ratios);

		_ratios = ratios;
	}

	private static void assertAllocatable(IEnumerable<Ratio> ratios)
	{
		decimal sum = ratios.Select(r => r.Value).Sum();
		if (!sum.Equals(decimal.One))
		{
			throw new ArgumentOutOfRangeException(nameof(ratios), sum, "Ratios have to sum up 1.0.");
		}
	}

	#endregion

	#region collection-like methods

	/// <inheritdoc />
	public IEnumerator<Ratio> GetEnumerator()
	{
		return ((IEnumerable<Ratio>)_ratios).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _ratios.GetEnumerator();
	}

	/// <summary>
	/// Gets a 32-bit integer that represents the total number of ratios in the <see cref="RatioCollection"/>.
	/// </summary>
	/// <returns>A 32-bit integer that represents the total number of ratios in the <see cref="RatioCollection"/>.</returns>
	public int Count => _ratios.Length;

	/// <summary>
	/// Gets the ratio at the specified index.
	/// </summary>
	/// <param name="index">The index of the ratio to get.</param>
	/// <returns>The ratio at the specified index.</returns>
	/// <exception cref="ArgumentOutOfRangeException">index is less than zero.
	/// -or-
	/// index is equal to or greater than <see cref="Count"/>.</exception>
	public Ratio this[int index] => _ratios[index];

	#endregion

	/// <summary>
	/// Returns a <see cref="string"/> that represents the current <see cref="RatioCollection"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="string"/> that represents the current <see cref="RatioCollection"/>.
	/// </returns>
	public override string ToString()
	{
		return Stringifier.Default.Stringify(_ratios);
	}

	/// <inheritdoc />
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return Stringifier.Default.Stringify(_ratios, r => r.ToString(format, formatProvider));
	}
}
