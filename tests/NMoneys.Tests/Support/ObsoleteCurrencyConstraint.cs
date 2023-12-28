using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Support;

public class ObsoleteCurrencyRaised : DelegatingConstraint
{
	public ObsoleteCurrencyRaised(byte timesRaised)
	{
		Delegate = Is.EqualTo(timesRaised);
	}

	protected override ConstraintResult matches(object current)
	{
		byte counter = 0;
		EventHandler <ObsoleteCurrencyEventArgs> callback = (sender, e) => { counter++; };

		Currency.ObsoleteCurrency += callback;

		try
		{
			((Action)current).Invoke();
		}
		finally
		{
			Currency.ObsoleteCurrency -= callback;
		}

		return Delegate.ApplyTo(counter);
	}
}
