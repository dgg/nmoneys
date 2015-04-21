using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests.Support
{
	[TestFixture]
	public class ThreadSafeCacheTester
	{
		private static readonly ThreadSafeCache<string, string> Sut = new ThreadSafeCache<string, string>();

		#region Issue 30. Case sensitivity. Money instances can be obtained by any casing of the IsoCode (Alphbetic code)

		[Test]
		public void Add_WhenNotThere_IsThreadSafe()
		{
			var list = new List<Task>();
			for (var i = 0; i < 100; i++)
				list.Add(new Task(addKey, "key"));
			try
			{
				list.ForEach(t => t.Start());
				Task.WaitAll(list.ToArray());
				Assert.Pass("No ThreadSafeCache erros");
			}
			catch (AggregateException exception)
			{
				var exceptions = new List<Exception>();
				innerExceptions(exception, exceptions);
				var sb = new StringBuilder();
				exceptions.Distinct(new ExceptionEqualityComparer()).ToList().ForEach(ex => sb.AppendLine(ex.ToString()));
				Assert.Fail(sb.ToString());
			}
		}

		private static void innerExceptions(AggregateException exception, IList<Exception> exceptions)
		{
			foreach (var ex in exception.InnerExceptions)
			{
				var agx = ex as AggregateException;
				if (agx != null)
					innerExceptions(agx, exceptions);
				else
					exceptions.Add(ex);
			}
		}


		private static void addKey(object obj)
		{
			var key = obj.ToString();
			string value;
			if (!Sut.TryGet(key, out value))
				Sut.Add(key, Guid.NewGuid().ToString());
		}

		class ExceptionEqualityComparer : IEqualityComparer<Exception>
		{
			public bool Equals(Exception x, Exception y)
			{
				return string.Equals(x.Message, y.Message, StringComparison.InvariantCultureIgnoreCase);
			}

			public int GetHashCode(Exception obj)
			{
				return obj.Message.GetHashCode();
			}
		}

		
		private static void getCurrency(object input)
		{
			var currencyIso = (CurrencyIsoCode)input;
			for (var i = 0; i < 10; i++)
				Currency.Get(currencyIso);
		}

		#endregion
	}
}