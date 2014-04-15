using System;
using NUnit.Framework;

namespace NMoneys.Tests.CustomConstraints
{
	internal class ObsoleteCurrencyRaisedConstraint : CustomConstraint<Action>
	{
		public ObsoleteCurrencyRaisedConstraint(uint timesRaised)
		{
			_inner = Is.EqualTo(timesRaised);
		}

		protected override bool matches(Action current)
		{
			uint counter = 0;
			EventHandler<ObsoleteCurrencyEventArgs> callback = (sender, e) => { counter++; };
			Currency.ObsoleteCurrency += callback;

			try
			{
				current();
			}
			finally
			{
				Currency.ObsoleteCurrency -= callback;
			}
			return _inner.Matches(counter);
		}
	}
}
