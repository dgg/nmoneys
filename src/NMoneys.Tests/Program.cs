using System;
using System.Reflection;
using NUnit.Common;
using NUnit.Framework;
using NUnitLite;

namespace NMoneys.Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			var writter = new ExtendedTextWrapper(Console.Out);
			int exitCode = new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args, writter, Console.In);
			Environment.Exit(exitCode);
		}
	}
}