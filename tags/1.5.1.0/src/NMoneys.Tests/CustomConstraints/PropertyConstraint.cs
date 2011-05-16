using System;
using System.Linq.Expressions;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class PropertyConstraint<T> : PropertyConstraint
	{
		public PropertyConstraint(Expression<Func<T, object>> property, Constraint baseConstraint) : base(nameOf(property), baseConstraint) { }

		private static string nameOf(Expression<Func<T, object>> propertyExpr)
		{
			MemberExpression memberExpression;
			switch (propertyExpr.Body.NodeType)
			{
				case ExpressionType.Convert:
					UnaryExpression body = (UnaryExpression)propertyExpr.Body;
					memberExpression = (MemberExpression)body.Operand;
					break;
				case ExpressionType.MemberAccess:
					memberExpression = (MemberExpression)propertyExpr.Body;
					break;
				default:
					throw new ArgumentException("Supplied expression does not represent a member.", "propertyExpr");
			}

			return memberExpression.Member.Name;
		}
	}
}
