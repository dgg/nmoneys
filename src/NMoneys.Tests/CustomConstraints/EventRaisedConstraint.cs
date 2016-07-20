using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class ObsoleteCurrencyRaisedConstraint : DelegatingConstraint
	{
		public ObsoleteCurrencyRaisedConstraint(uint timesRaised)
		{
			Delegate = Is.EqualTo(timesRaised);
		}

		protected override ConstraintResult matches(object current)
		{
			uint counter = 0;
			EventHandler<ObsoleteCurrencyEventArgs> callback = (sender, e) => { counter++; };
			Currency.ObsoleteCurrency += callback;

			try
			{
				((Action)current)();
			}
			finally
			{
				Currency.ObsoleteCurrency -= callback;
			}
			return Delegate.ApplyTo(counter);
		}
	}
}
