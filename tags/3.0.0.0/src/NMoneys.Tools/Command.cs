using System;

namespace NMoneys.Tools
{
	internal abstract class Command
	{
		protected abstract void DoExecute();
		public void Execute()
		{
			try
			{
				DoExecute();
			}
			catch (Exception e)
			{
				string error = string.Format("---\nThe following error occurred while executing the snippet:\n{0}\n---", e);
				Console.WriteLine(error);
			}
			finally
			{
				Console.Write("Press any key to continue...");
				Console.ReadKey();
			}
		}

		protected void WL(object text, params object[] args)
		{
			Console.WriteLine(text.ToString(), args);
		}

		protected string RL()
		{
			return Console.ReadLine();
		}
	}
}
