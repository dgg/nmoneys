using System;
using ConsoleTables;

namespace NMoneys.Tools.CompareSingle
{
	internal class DiscrepanciesTable
	{
		private readonly ConsoleTable _table;

		protected DiscrepanciesTable(string column1, string column2, string column3)
		{
			_table = new ConsoleTable(column1, column2, column3);
		}

		protected void MaybeWrite(string header)
		{
			var count = _table.Rows.Count;

			Console.Write($"[{count}] ");
			Console.WriteLine(header);
			if (count > 0) _table.Write(Format.Alternative);
		}

		protected void AddRow(string value1, string value2, string value3)
		{
			_table.AddRow(value1, value2, value3);
		}
	}
}