using System.IO;

namespace NMoneys.Support
{
	public static class IO
	{
		public static void Finalize(this Stream stream)
		{
#if NET
			stream.Close();
#endif
			stream.Dispose();
		}
	}
}