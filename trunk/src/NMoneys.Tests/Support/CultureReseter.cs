using System;
using System.Globalization;
using System.Threading;

namespace NMoneys.Tests.Support
{
	internal class CultureReseter : IDisposable
	{
		private readonly CultureInfo _threadCulture, _threadUICulture;

		public CultureReseter()
		{
			backUpThreadCulture(out _threadCulture, out _threadUICulture);
		}

		private CultureReseter set(CultureInfo threadCulture, CultureInfo threadUICulture)
		{
			setThreadCulture(threadCulture, threadUICulture);
			return this;
		}

		public static CultureReseter Set(CultureInfo threadCulture, CultureInfo threadUICulture)
		{
			return new CultureReseter().set(threadCulture, threadUICulture);
		}

		public static CultureReseter Set(string cultureName, string uicultureName)
		{
			return Set(new CultureInfo(cultureName), new CultureInfo(uicultureName));
		}

		public static CultureReseter Set(CultureInfo bothCultures)
		{
			return new CultureReseter().set(bothCultures, bothCultures);
		}

		public static CultureReseter Set(string bothCultureName)
		{
			return Set(new CultureInfo(bothCultureName));
		}

		public void Dispose()
		{
			setThreadCulture(_threadCulture, _threadUICulture);
		}

		private static void backUpThreadCulture(out CultureInfo threadCulture, out CultureInfo threadUICulture)
		{
			threadCulture = CultureInfo.CurrentCulture;
			threadUICulture = CultureInfo.CurrentUICulture;
		}

		private static void setThreadCulture(CultureInfo threadCulture, CultureInfo threadUICulture)
		{
			Thread.CurrentThread.CurrentCulture = threadCulture;
			Thread.CurrentThread.CurrentUICulture = threadUICulture;
		}
	}
}
