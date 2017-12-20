using System;
using System.Linq.Expressions;

namespace NMoneys.Tools.CompareGlobalization
{
	internal class Diff
	{
		public Diff(string propertyName, string globalization, string configuration)
		{
			PropertyName = propertyName;
			Globalization = globalization;
			Configuration = configuration;
		}

		public string PropertyName { get; }
		public string Globalization { get; }
		public string Configuration { get; }

		public static Diff For<T>(CurrencyInfo fromGlobalization, CurrencyInfo fromConfiguration,
			Expression<Func<CurrencyInfo, T>> member, Func<T, T, bool> equality)
		{
			Diff diff = null;
			var valueOf = member.Compile();
			T globalizationValue = valueOf(fromGlobalization),
				configurationValue = valueOf(fromConfiguration);
			if (!equality(globalizationValue, configurationValue))
			{

				diff = new Diff(Name.Of(member), globalizationValue.ToString(), configurationValue.ToString());
			}
			return diff;
		}
	}
}