using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NMoneys.Serialization
{

	/// <summary>
	/// Provides the methods needed to substitute one type for another in order to customize the serialization and deserialization processes
	/// performed by <see cref="DataContractJsonSerializer"/>
	/// </summary>
	/// <remarks>Schema-related opertions are not implemented as it is geared towards JSON serialization.</remarks>
	public class DataContractSurrogate : IDataContractSurrogate
	{
		#region data for DataContractJsonSerializer constructor
		
		/// <summary>
		/// The maximum number of items in the graph to serialize or deserialize when serializing just types from <see cref="NMoneys"/>.
		/// </summary>
		public readonly int MaxItemsInObjectGraph = 4;

		/// <summary>
		/// The type of the instance that is serialized or deserialized.
		/// </summary>
		/// <typeparam name="T">The type from <see cref="NMoneys"/> being serialized or deserialized.</typeparam>
		/// <returns>The type of the instance that is serialized or deserialized.</returns>
		public Type Type<T>()
		{
			return typeof(T);
		}

		/// <summary>
		/// An <see cref="IEnumerable{T}"/> of <see cref="Type"/> that contains the type present in the object graph.
		/// </summary>
		/// <typeparam name="T">The type from <see cref="NMoneys"/> being serialized or deserialized.</typeparam>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Type"/> that contains the type present in the object graph.</returns>
		public IEnumerable<Type> KnownTypes<T>()
		{
			return new[] { Type<T>() };
		}
		/// <summary>
		/// Do not ignore the <see cref="IExtensibleDataObject"/> interface upon serialization or unexpected data upon deserialization.
		/// </summary>
		public readonly bool IgnoreExtensionDataObject = false;

		/// <summary>
		/// Do not to emit type information.
		/// </summary>
		public readonly bool AlwaysEmitTypeInformation = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataContractJsonSerializer"/> class to serialize or deserialize an object of the specified type.
		/// </summary>
		/// <remarks>This method also specifies a list of known types that may be present in the object graph, the maximum number of graph items to
		/// serialize or deserialize, whether to ignore unexpected data or emit type information, and a surrogate for custom serialization according to the
		/// options defined in <see cref="Type{T}"/>, <see cref="KnownTypes{T}"/>, <see cref="MaxItemsInObjectGraph"/>, <see cref="IgnoreExtensionDataObject"/>,
		/// <see cref="AlwaysEmitTypeInformation"/> and the current <see cref="IDataContractSurrogate"/>.</remarks>
		/// <typeparam name="T">The type from <see cref="NMoneys"/> being serialized or deserialized.</typeparam>
		/// <returns>An instance of <see cref="DataContractJsonSerializer"/> initialied with the default options in <see cref="DataContractSurrogate"/></returns>
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

		/// <summary>
		/// During serialization, deserialization, returns a data contract type that substitutes the specified type. 
		/// </summary>
		/// <returns>
		/// The datacontract-serialiable <see cref="Type"/> to substitute for the <paramref name="type"/> value.
		/// </returns>
		/// <param name="type">The CLR type <see cref="Type"/> to substitute. </param>
		public Type GetDataContractType(Type type)
		{
			if (type == typeof(Money)) return typeof(Data.Money);
			if (type == typeof(Currency)) return typeof(Data.Currency);
			return type;
		}

		/// <summary>
		/// During serialization, returns an object that substitutes the specified object. 
		/// </summary>
		/// <returns>
		/// The substituted datacontract-serializable object that will be serialized. 
		/// </returns>
		/// <param name="obj">The object to substitute. </param>
		/// <param name="targetType">The <see cref="Type"/> that the substituted object should be assigned to.</param>
		public object GetObjectToSerialize(object obj, Type targetType)
		{
			if (obj is Money && targetType == typeof(Data.Money))
			{
				return new Data.Money((Money)obj);
			}
			if (obj is Currency && targetType == typeof(Data.Currency))
			{
				return new Data.Currency((Currency)obj);
			}
			return obj;
		}

		/// <summary>
		/// During deserialization, returns an object that is a substitute for the specified object.
		/// </summary>
		/// <returns>
		/// The substituted deserialized datacontract-serializable object.
		/// </returns>
		/// <param name="obj">The deserialized object to be substituted.</param>
		/// <param name="targetType">The <see cref="Type"/> that the substituted object should be assigned to.</param>
		public object GetDeserializedObject(object obj, Type targetType)
		{
			if (obj is Data.Money && targetType == typeof(Money))
			{
				return ((Data.Money)obj).RevertSurrogation();
			}
			if (obj is Data.Currency && targetType == typeof(Currency))
			{
				return ((Data.Currency)obj).RevertSurrogation();
			}
			return obj;
		}

		/// <summary>
		/// During schema export operations, inserts annotations into the schema for non-null return values. 
		/// </summary>
		/// <remarks>This method is not implemented, as the surrogate is designed to  work with schema-less JSON serialization.</remarks>
		/// <returns>
		/// An object that represents the annotation to be inserted into the XML schema definition. 
		/// </returns>
		/// <param name="memberInfo">A <see cref="MemberInfo"/> that describes the member.</param>
		/// <param name="dataContractType">A <see cref="Type"/>.</param>
		public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		/// <summary>
		/// During schema export operations, inserts annotations into the schema for non-null return values. 
		/// </summary>
		/// <remarks>This method is not implemented, as the surrogate is designed to  work with schema-less JSON serialization.</remarks>
		/// <returns>
		/// An object that represents the annotation to be inserted into the XML schema definition. 
		/// </returns>
		/// <param name="clrType">The CLR type to be replaced.</param>
		/// <param name="dataContractType">The data contract type to be annotated.</param>
		public object GetCustomDataToExport(Type clrType, Type dataContractType)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		/// <summary>
		/// Sets the collection of known types to use for serialization and deserialization of the custom data objects. 
		/// </summary>
		/// <remarks>This method is not implemented, as the surrogate is designed to  work with schema-less JSON serialization.</remarks>
		/// <param name="customDataTypes">A <see cref="Collection{T}"/>  of <see cref="Type"/> to add known types to.</param>
		public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		/// <summary>
		/// During schema import, returns the type referenced by the schema.
		/// </summary>
		/// <remarks>This method is not implemented, as the surrogate is designed to  work with schema-less JSON serialization.</remarks>
		/// <returns>
		/// The <see cref="Type"/> to use for the referenced type.
		/// </returns>
		/// <param name="typeName">The name of the type in schema.</param>
		/// <param name="typeNamespace">The namespace of the type in schema.</param>
		/// <param name="customData">The object that represents the annotation inserted into the XML schema definition,
		/// which is data that can be used for finding the referenced type.</param>
		public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}

		/// <summary>
		/// Processes the type that has been generated from the imported schema.
		/// </summary>
		/// <remarks>This method is not implemented, as the surrogate is designed to  work with schema-less JSON serialization.</remarks>
		/// <returns>
		/// A <see cref="System.CodeDom.CodeTypeDeclaration"/> that contains the processed type.
		/// </returns>
		/// <param name="typeDeclaration">A <see cref="System.CodeDom.CodeTypeDeclaration"/> to process that represents the type declaration
		/// generated during schema import.</param>
		/// <param name="compileUnit">The <see cref="System.CodeDom.CodeCompileUnit"/> that contains the other code generated during schema import.</param>
		public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
		{
			throw new NotImplementedException("See examples of usage or report issue.");
		}
	}
}
