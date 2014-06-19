using System;
using NUnit.Framework;
using Guard = NMoneys.Support.Guard;

namespace NMoneys.Tests.Support
{
	[TestFixture]
	public class GuardTester
	{
		[Test]
		public void AgainstNullArgument_FalseCondition_NoException()
		{
			string param = "notNull";
			Assert.That(() => Guard.AgainstNullArgument("param", param), Throws.Nothing);
			Assert.That(() => new GuardSubject().Method("notNull"), Throws.Nothing);
		}

		[Test]
		public void AgainstNullArgument_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = null, paramName = "param", actualMessage = new ArgumentNullException(paramName).Message;

			Assert.That(() => Guard.AgainstNullArgument(paramName, param),
				Throws.InstanceOf<ArgumentNullException>()
				.With.Property("Message").EqualTo(actualMessage).IgnoreCase
				.And.With.Property("ParamName").EqualTo(paramName));

			Assert.That(() => new GuardSubject().Method(null),
				Throws.InstanceOf<ArgumentNullException>()
				.With.Property("Message").EqualTo(actualMessage).IgnoreCase
				.And.With.Property("ParamName").EqualTo(paramName));
		}

		[Test]
		public void AgainstArgument_FalseCondition_NoException()
		{
			Assert.That(() => Guard.AgainstArgument("param", "asd" == null, "no Exception"), Throws.Nothing);
		}

		[Test]
		public void AgainstArgument_TrueConditionMessage_ExceptionWithMessageAndParam()
		{
			string message = "message", param = "param";
			bool trueCondition = 3 > 2;

			Assert.That(() => Guard.AgainstArgument(param, trueCondition, message),
				Throws.ArgumentException.With.Property("Message").StringContaining(message).And.With.Property("ParamName").EqualTo(param));
		}

		[Test]
		public void AgainstArgumentNoMessage_FalseCondition_NoException()
		{
			Assert.That(() => Guard.AgainstArgument("param", "asd" == null), Throws.Nothing);
		}

		[Test]
		public void AgainstArgumentNoMessage_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = "param", actualMessage = new ArgumentException(null, param).Message;
			bool trueCondition = 3 > 2;

			Assert.That(() => Guard.AgainstArgument(param, trueCondition),
				Throws.ArgumentException.With.Property("Message").EqualTo(actualMessage).IgnoreCase
				.And.With.Property("ParamName").EqualTo(param));
		}

		internal class GuardSubject
		{
			public void Method(string param)
			{
				Guard.AgainstNullArgument("param", param);
			}
		}
	}
}
