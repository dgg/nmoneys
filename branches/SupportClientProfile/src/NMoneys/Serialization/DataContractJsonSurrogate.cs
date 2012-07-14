using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NMoneys.Serialization
{
	
	public class DataContractJsonSurrogate : IDataContractSurrogate
	{
		#region data for DataContractJsonSerializer constructor

		public readonly int MaxItemsInObjectGraph = 4;
		public Type Type<T>()
		{
			return typeof (T);
		}
		public IEnumerable<Type> KnownTypes<T>()
		{
			return new [] {Type<T>()};
		}

		public readonly bool IgnoreExtensionDataObject = false;
		public readonly bool AlwaysEmitTypeInformation = false;

		public DataContractJsonSerializer BuildSerializer<T>()
		{
			var serializer = new DataContractJsonSerializer(
				Type<T>(),
				KnownTypes<T>(),
				MaxItemsInObjectGraph,
				IgnoreExtensionDataObject,
				this,
				AlwaysEmitTypeInformation);
			return serializer;
		}

		#endregion

		public Type GetDataContractType(Type type)
		{
			if (type == typeof (Money)) return typeof(Data.Money);
			if (type == typeof(Currency)) return typeof (Data.Currency);
			return type;
		}

		public object GetObjectToSerialize(object obj, Type targetType)
		{
			if (obj is Money && targetType == typeof(Data.Money))
			{
				return new Data.Money((Money) obj);
			}
			if (obj is Currency && targetType == typeof (Data.Currency))
			{
				return new Data.Currency((Currency) obj);
			}
			return obj;
		}

		public object GetDeserializedObject(object obj, Type targetType)
		{
			if (obj is Data.Money && targetType == typeof(Money))
			{
				return ((Data.Money) obj).RevertSurrogation();
			}
			if (obj is Data.Currency && targetType == typeof(Currency))
			{
				return ((Data.Currency)obj).RevertSurrogation();
			}
			return obj;
		}

		public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		public object GetCustomDataToExport(Type clrType, Type dataContractType)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}
	}
}
