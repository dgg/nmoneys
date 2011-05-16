using System;
using System.Globalization;
using System.Linq;

namespace NMoneys.Support
{
	/// <summary>
	/// Holds the different ways of representing the number of digits in each group to the left of the decimal in a number. 
	/// </summary>
	internal class GroupSizes : TokenizedValue
	{
		/// <summary>
		/// The number of digits in each group to the left of the decimal in a number.
		/// </summary>
		public int[] Sizes { get; private set; }

		/// <summary>
		/// Space-tokenized representation of the number of digits in each group to the left of the decimal in a number.
		/// </summary>
		public string TokenizedSizes { get; private set; }

		private GroupSizes(int[] sizes, string tokenizedSizes)
		{
			Sizes = sizes;
			TokenizedSizes = tokenizedSizes;
		}

		/// <summary>
		/// Creates a instance of <see cref="GroupSizes"/> from its the space-tokenized representation.
		/// </summary>
		/// <param name="tokenizedSizes">Space-tokenized representation of the number of digits in each group to the left of the decimal in a number.</param>
		/// <returns>Instance with all representations of the number of digits in each group.</returns>
		public static GroupSizes FromTokenizedSizes(string tokenizedSizes)
		{
			Guard.AgainstNullArgument("tokenizedSizes", tokenizedSizes);

			int[] sizes = new int[0];
			if (!string.IsNullOrEmpty(tokenizedSizes))
			{
				sizes = split(tokenizedSizes).Select(s => Convert.ToInt32(s)).ToArray();
			}
			assertCorrectSizes(sizes);
			return new GroupSizes(sizes, tokenizedSizes);
		}


		/// <summary>
		/// Creates an instance of <see cref="GroupSizes"/> from the number of digits in each group to the left of the decimal in a number.
		/// </summary>
		/// <param name="sizes">The number of digits in each group to the left of the decimal in a number.</param>
		/// <returns>Instance with all representations of the number of digits in each group.</returns>
		public static GroupSizes FromSizes(int[] sizes)
		{
			//Guard.AgainstNullArgument("sizes", sizes);
			assertCorrectSizes(sizes);
			return new GroupSizes(sizes, join(sizes.Select(s => s.ToString()).ToArray()));
		}

		private static void assertCorrectSizes(int[] sizes)
		{
			try
			{
				new NumberFormatInfo { CurrencyGroupSizes = sizes };
			}
			catch (ArgumentNullException)
			{
				throw;
			}
			catch (ArgumentException ex)
			{
				throw new ArgumentException(ex.Message, "sizes");
			}
		}
	}
}
