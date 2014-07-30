using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET
{
	internal interface IMoneyReader
	{
		decimal ReadAmount(JToken token);
		CurrencyIsoCode ReadCurrencyCode(JToken token);
	}

	internal class DefaultMoneyReader : MoneyOperator, IMoneyReader
	{
		private readonly CurrencyStyle _style;

		public DefaultMoneyReader(CurrencyStyle style, IContractResolver resolver) : base(resolver)
		{
			_style = style;
		}

		public decimal ReadAmount(JToken token)
		{
			return token[_amount].Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token)
		{
			JToken currency = token[_currency];
			return currency.GetValue(_style);
		}
	}

	internal class CanonicalMoneyReader : MoneyOperator, IMoneyReader
	{
		public CanonicalMoneyReader(IContractResolver resolver) : base(resolver) { }

		public decimal ReadAmount(JToken token)
		{
			return token[_amount].Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token)
		{
			JToken isoCode = token[_currency][_isoCode];
			return isoCode.GetValue(CurrencyStyle.Alphabetic);
		}
	}

	internal class CurrencyLessMoneyReader : MoneyOperator, IMoneyReader
	{
		public CurrencyLessMoneyReader(IContractResolver resolver) : base(resolver) { }
		public decimal ReadAmount(JToken token)
		{
			return token.Type == JTokenType.Object && token.HasValues ?
				token.Values().First().Value<decimal>() :
				token.Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token)
		{
			return CurrencyIsoCode.XXX;
		}
	}

	internal static partial class CurrencyStyleExtensions
	{
		internal static CurrencyIsoCode GetValue(this JToken token, CurrencyStyle style)
		{
			CurrencyIsoCode value;
			switch (style)
			{
				case CurrencyStyle.Alphabetic:
					var alphabetic = token.Value<string>();
					value = Currency.Code.Parse(alphabetic);
					break;
				case CurrencyStyle.Numeric:
					var numeric = token.Value<short>();
					value = Currency.Code.Cast(numeric);
					break;
				default:
					throw new ArgumentOutOfRangeException("style");
			}
			return value;
		}
	}
}