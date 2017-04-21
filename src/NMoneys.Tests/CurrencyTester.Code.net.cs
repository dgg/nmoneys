using System;
using System.Collections.Generic;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	// PlatformAttribute is not present in NUnits netstandard build, but since the tests marked 
	// as platform-dependent cannot happen in a core runtime (they apply to old .NET 2 versions), 
	// they can be safely ignored
	[TestFixture]
	public partial class CurrencyTester
	{
		#region Comparer

		[Test, NUnit.Framework.Category("Performance"), Platform(Include = "Net-2.0")]
		public void Comparer_BetterPerformance_ThanDefaultComparer()
		{
			CurrencyIsoCode[] values = Enumeration.GetValues<CurrencyIsoCode>();

			int iterations = 1000000;
			TimeSpan fast = run(iterations, i => 
				populateDictionary(new Dictionary<CurrencyIsoCode, int>(Currency.Code.Comparer), values, i));
			TimeSpan @default = run(iterations, i => 
				populateDictionary(new Dictionary<CurrencyIsoCode, int>(EqualityComparer<CurrencyIsoCode>.Default), values, i));

			Assert.That(fast, Is.LessThan(@default), "{0} < {1}", fast, @default);

			// not only faster, more than 5 times faster
			Assert.That(fast.TotalMilliseconds * 5, Is.LessThan(@default.Milliseconds), "{0} << {1}", fast, @default);
		}

		#endregion
	}
}