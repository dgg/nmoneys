using System.Globalization;
using ConsoleTables;

namespace NMoneys.Tools.CompareGlobalization
{
	internal abstract class ComparisonTable
	{
		private readonly ConsoleTable _table;
		protected ComparisonTable(params string[] columns)
		{
			_table = new ConsoleTable(columns);
		}

		protected ComparisonTable AddRow(params object[] values)
		{
			_table.AddRow(values);
			return this;
		}

		public void Print(GlobalizationCurrencyInfo[] globalizationInfo, CurrencyInfo[] configurationInfo)
		{
			BuildTable(globalizationInfo, configurationInfo);

			_table.Write(Format.Alternative);
		}

		protected abstract void BuildTable(GlobalizationCurrencyInfo[] globalizationInfo, CurrencyInfo[] configurationInfo);

		protected string FormatCultureColumn(CultureInfo culture, bool? isOverwritten = null)
		{
			string overwritten = isOverwritten.GetValueOrDefault() ? " *" : string.Empty;
			string column = $"{culture.Name} [{culture.EnglishName}]{overwritten}";
			return column;
		}
	}
}