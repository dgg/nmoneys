using System;
using System.Linq.Expressions;

namespace NMoneys.Tools.CompareGlobalization
{
	internal static class Name
	{
		public static string Of<TObject, TValue>(Expression<Func<TObject, TValue>> propertyExpr)
		{
			return getMemberExpression(propertyExpr).Member.Name;
		}

		private static MemberExpression getMemberExpression<TObject, TValue>(Expression<Func<TObject, TValue>> expression)
		{
			MemberExpression memberExpression = null;
			if (expression.Body.NodeType == ExpressionType.Convert)
			{
				var body = (UnaryExpression)expression.Body;
				memberExpression = body.Operand as MemberExpression;
			}
			else if (expression.Body.NodeType == ExpressionType.MemberAccess)
			{
				memberExpression = expression.Body as MemberExpression;
			}

			return memberExpression;
		}
	}
}