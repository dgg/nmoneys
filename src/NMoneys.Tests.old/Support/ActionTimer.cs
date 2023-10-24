using System;
using System.Diagnostics;

namespace NMoneys.Tests.Support
{
	internal static class ActionTimer
	{
		public static TimeSpan Time(Action action)
		{
			Stopwatch watch = Stopwatch.StartNew();
			Time(watch, action);
			return watch.Elapsed;
		}

		/// <summary>
		/// Times the execution time of <paramref name="numberOfIterations"/> the given delegate.
		/// </summary>
		/// <param name="watch">Instance of watch to measure time with</param>
		/// <param name="action">The delegate to time</param>
		/// <param name="numberOfIterations">Number of times the delegate must be executed</param>
		public static void Time(Stopwatch watch, Action action, long numberOfIterations)
		{
			watch.Reset();
			watch.Start();

			for (int i = 0; i < numberOfIterations; i++)
			{
				action();
			}

			watch.Stop();
		}

		/// <summary>
		/// Times the execution time of the given delegate.
		/// </summary>
		/// <param name="watch">Instance of watch to measure time with</param>
		/// <param name="action">The delegate to time</param>
		public static void Time(Stopwatch watch, Action action)
		{
			Time(watch, action, 1);
		}
	}
}
