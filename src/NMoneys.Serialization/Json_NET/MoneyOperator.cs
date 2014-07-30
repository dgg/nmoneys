using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET
{
	internal abstract class MoneyOperator
	{
		protected readonly string _amount, _currency, _money, _isoCode;
		protected MoneyOperator(IContractResolver resolver)
		{
			_amount = "Amount";
			_currency = "Currency";
			_money = "Money";
			_isoCode = "IsoCode";

			var contractResolver = resolver as DefaultContractResolver;
			if (contractResolver != null)
			{
				_amount = contractResolver.GetResolvedPropertyName(_amount);
				_currency = contractResolver.GetResolvedPropertyName(_currency);
				_isoCode = contractResolver.GetResolvedPropertyName(_isoCode);
			}
		}
	}
}