using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace NMoneys.Support.Ext
{
	internal static class Extensions
	{
		internal static string ToDelimitedString<T>(this IEnumerable<T> source)
		{
			return toDelimitedString(source, ", ", t => t.ToString());
		}

		internal static string ToDelimitedString<T>(this IEnumerable<T> source, Func<T, string> toString)
		{
			return toDelimitedString(source, ", ", toString);
		}

		private static string toDelimitedString<T>(IEnumerable<T> source, string delimiter, Func<T, string> toString)
		{
			if (source == null) return null;

			StringBuilder sb = new StringBuilder();
			foreach (var item in source)
			{
				sb.Append(toString(item));
				sb.Append(delimiter);
			}
			if (sb.Length >= delimiter.Length)
			{
				sb.Remove(sb.Length - delimiter.Length, delimiter.Length);
			}
			return sb.ToString();
		}

		internal static bool IsEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		private static string[] emptyIfNull(this string[] s)
		{
			return s ?? new string[0];
		}

		/// <summary>
		/// Converts and array of unicode representations into its string
		/// </summary>
		/// <remarks>Optimized for a low number of unicodes (&lt;10)</remarks>
		internal static string FromUnicodes(this string[] unicodes)
		{
			return unicodes.emptyIfNull().Aggregate(string.Empty, 
				(current, item) => current + char.ConvertFromUtf32(Convert.ToInt32(item.Trim(), CultureInfo.InvariantCulture)));
		}

		/// <summary>
		/// Allows translating a currency negative pattern <c>[0..15]</c> to a number negative pattern <c>[0..4]</c>
		/// maintaining overall layout
		/// </summary>
		internal static int TranslateNegativePattern(this int currencyNegativePattern)
		{
			int numberNegativePattern = -1;
			switch (currencyNegativePattern)
			{
				case 0:
				case 4:
				case 14:
				case 15:
					numberNegativePattern = 0;
					break;
				case 1:
				case 2:
				case 5:
				case 8:
				case 12:
					numberNegativePattern = 1;
					break;
				case 9:
					numberNegativePattern = 2;
					break;
				case 3:
				case 6:
				case 7:
				case 11:
				case 13:
					numberNegativePattern = 3;
					break;
				case 10:
					numberNegativePattern = 4;
					break;
				default:
					new Range<int>(0.Close(), 15.Close()).AssertArgument(nameof(currencyNegativePattern), currencyNegativePattern);
					break;
			}
			return numberNegativePattern;
		}

		public static string SelectMandatory(this XPathNavigator node, XPathExpression selector)
		{
			return node.SelectMandatory(selector, n => n.Value);
		}

		public static T SelectMandatory<T>(this XPathNavigator node, XPathExpression selector, Func<XPathNavigator, T> value)
		{
			return value(node.SelectSingleNode(selector));
		}

		public static T SelectOptional<T>(this XPathNavigator node, XPathExpression selector, Func<XPathNavigator, T> value)
		{
			return SelectOptional(node, selector, value, default(T));
		}

		public static T SelectOptional<T>(this XPathNavigator node, XPathExpression selector, Func<XPathNavigator, T> value, T defaultValue)
		{
			XPathNavigator found = node.SelectSingleNode(selector);
			return found != null ? value(found) : defaultValue;
		}
	}
}
