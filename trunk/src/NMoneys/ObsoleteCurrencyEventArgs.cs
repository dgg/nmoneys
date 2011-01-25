using System;

namespace NMoneys
{
	/// <summary>
	/// 
	/// </summary>
	public class ObsoleteCurrencyEventArgs : EventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		public ObsoleteCurrencyEventArgs(CurrencyIsoCode code)
		{
			Code = code;
		}

		/// <summary>
		/// 
		/// </summary>
		public CurrencyIsoCode Code { get; private set; }
	}
}