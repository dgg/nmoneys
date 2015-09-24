using System;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class ObsoleteCurrencyRaisedConstraint : DelegatingConstraint<Action>
	{
		public ObsoleteCurrencyRaisedConstraint(uint timesRaised)
		{
			Delegate = Is.EqualTo(timesRaised);
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
			return Delegate.Matches(counter);
		}
	}
}
