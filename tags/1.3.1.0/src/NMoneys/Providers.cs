using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml.XPath;
using NMoneys.Support.Ext;

namespace NMoneys
{
	internal interface ICurrencyInfoProvider
	{
		CurrencyInfo Get(CurrencyIsoCode code);
	}

	internal interface ICurrencyInfoInitializer : ICurrencyInfoProvider, IDisposable { }

	internal static class EmbeddedXml
	{
		private static readonly string _resourceName = typeof(Currency).Namespace + "." + "Currencies.xml";

		internal static Stream GetStream()
		{
			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_resourceName);
			if (stream == null) throw new MissingManifestResourceException(_resourceName);
			return stream;
		}

		internal static CurrencyInfo ReadInfo(XPathNavigator navigator, CurrencyIsoCode code)
		{
			XPathNavigator node = navigator.SelectSingleNode(string.Format("/currencies/currency[@code='{0}']", code));
			if (node == null) throw new MissconfiguredCurrencyException(code);

			CurrencyInfo info = new CurrencyInfo(
				code,
				node.SelectMandatory(englishName),
				node.SelectMandatory(nativeName),
				node.SelectMandatory(symbol),
				node.SelectMandatory(significantDecimalDigits, n => n.ValueAsInt),
				node.SelectMandatory(decimalSeparator),
				node.SelectMandatory(groupSeparator),
				node.SelectMandatory(groupSizes),
				node.SelectMandatory(positivePattern, n => n.ValueAsInt),
				node.SelectMandatory(negativePattern, n => n.ValueAsInt),
				node.SelectOptional(obsolete, n => n.ValueAsBoolean),
				node.SelectOptional(entity,
				n => CurrencyCharacterReferences.Get(n.Value),
				CharacterReference.Empty));

			return info;
		}

		internal static readonly XPathExpression englishName,
				nativeName,
				symbol,
				significantDecimalDigits,
				decimalSeparator,
				groupSeparator,
				groupSizes,
				positivePattern,
				negativePattern,
				obsolete,
				entity;

		static EmbeddedXml()
		{
			englishName = XPathExpression.Compile("englishName");
			nativeName = XPathExpression.Compile("nativeName");
			symbol = XPathExpression.Compile("symbol");
			significantDecimalDigits = XPathExpression.Compile("significantDecimalDigits");
			decimalSeparator = XPathExpression.Compile("decimalSeparator");
			groupSeparator = XPathExpression.Compile("groupSeparator");
			groupSizes = XPathExpression.Compile("groupSizes");
			positivePattern = XPathExpression.Compile("positivePattern");
			negativePattern = XPathExpression.Compile("negativePattern");
			obsolete = XPathExpression.Compile("obsolete");
			entity = XPathExpression.Compile("entity");
		}
	}

	internal class EmbeddedXmlProvider : ICurrencyInfoProvider
	{
		public CurrencyInfo Get(CurrencyIsoCode code)
		{
			using (Stream stream = EmbeddedXml.GetStream())
			{
				XPathDocument doc = new XPathDocument(stream);

				CurrencyInfo info = EmbeddedXml.ReadInfo(doc.CreateNavigator(), code);

				stream.Close();

				return info;
			}
		}
	}

	internal class EmbeddedXmlInitializer : ICurrencyInfoInitializer
	{
		private readonly Stream _stream;
		private readonly XPathNavigator _navigator;
		public EmbeddedXmlInitializer()
		{
			_stream = EmbeddedXml.GetStream();
			XPathDocument doc = new XPathDocument(_stream);
			_navigator = doc.CreateNavigator();
		}

		public CurrencyInfo Get(CurrencyIsoCode code)
		{
			return EmbeddedXml.ReadInfo(_navigator, code);
		}

		public void Dispose()
		{
			if (_stream != null)
			{
				_stream.Close();
				_stream.Dispose();
			}
		}
	}
}
