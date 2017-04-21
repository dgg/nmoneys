using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NMoneys.Support
{
	internal static class Reflect
	{
		public static Stream ResourceInSameAssembly<T>(string name)
		{
			Type type = typeof(T);
			Assembly assembly =
#if NET
				type.Assembly;
#else
				type.GetTypeInfo().Assembly;
#endif
			return assembly.GetManifestResourceStream(name);
		}

		public static MethodInfo Method(Type type, string methodName)
		{
#if NET
			return type.GetMethod(methodName);
#else
			return type.GetTypeInfo().GetDeclaredMethod(methodName);
#endif
		}

		public static bool IsEnum(Type type)
		{
#if NET
			return type.IsEnum;
#else
			return type.GetTypeInfo().IsEnum;
#endif
		}

		public static FieldInfo Field<T>(string fieldName)
		{
			Type type = typeof(T);
#if NET
			return type.GetField(fieldName);
#else
			return type.GetTypeInfo().GetDeclaredField(fieldName);
#endif
		}

		public static Attribute[] Attributes<T>(MemberInfo member, bool inherit = false) where T : Attribute
		{
#if NET
			return member.GetCustomAttributes(typeof(T), inherit).Cast<Attribute>().ToArray();
#else
			return member.GetCustomAttributes(typeof(T), inherit).ToArray();
#endif
		}
	}
}