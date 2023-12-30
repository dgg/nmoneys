using System.Linq;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	internal sealed class NotCanonicalTable : ComparisonTable
	{
		public NotCanonicalTable() : base("Code", "Globalization") { }

		protected override void BuildTable(GlobalizationCurrencyInfo[] globalizationInfo, CurrencyInfo[] configurationInfo)
		{
			var notDecorated = configurationInfo
				.Where(i => !Enumeration.HasAttribute<CurrencyIsoCode, CanonicalCultureAttribute>(i.Code))
				.ToArray();

			var cmp = new AlphaComparer();
			foreach (var fromConfiguration in notDecorated.OrderBy(i => i.Code, cmp))
			{
				var fromGlobalization = globalizationInfo.Where(i => Currency.Code.Comparer.Equals(i.Info.Code, fromConfiguration.Code)).ToArray();
				for (int i = 0; i < fromGlobalization.Length; i++)
				{
					if (i == 0)
					{
						AddRow(fromConfiguration.Code, string.Empty);
					}
					string fromGlobalizationColumn = FormatCultureColumn(fromGlobalization[i].Culture);

					AddRow(string.Empty, fromGlobalizationColumn);
				}
			}
		}
	}
}