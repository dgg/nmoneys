using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	public abstract class CustomConstraint<T> : Constraint
	{
		protected Constraint _inner;

		public override bool Matches(object current)
		{
			return matches((T)current);
		}

		protected abstract bool matches(T current);
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			_inner.WriteDescriptionTo(writer);
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			_inner.WriteActualValueTo(writer);
		}

		public override void WriteMessageTo(MessageWriter writer)
		{
			_inner.WriteMessageTo(writer);
		}
	}
}
