using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml.XPath;

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
				node.SelectSingleNode(EmbeddedXml.englishName).Value,
				node.SelectSingleNode(EmbeddedXml.nativeName).Value,
				node.SelectSingleNode(EmbeddedXml.symbol).Value,
				node.SelectSingleNode(EmbeddedXml.significantDecimalDigits).ValueAsInt,
				node.SelectSingleNode(EmbeddedXml.decimalSeparator).Value,
				node.SelectSingleNode(EmbeddedXml.groupSeparator).Value,
				node.SelectSingleNode(EmbeddedXml.groupSizes).Value,
				node.SelectSingleNode(EmbeddedXml.positivePattern).ValueAsInt,
				node.SelectSingleNode(EmbeddedXml.negativePattern).ValueAsInt);

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
				negativePattern;

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

	internal class ConfigurationProvider : ICurrencyInfoProvider
	{
		public CurrencyInfo Get(CurrencyIsoCode code)
		{
			return null;
		}
	}

	// use if only configuration is very memory consuming
	internal class AttibuteProvider : ICurrencyInfoProvider
	{
		public CurrencyInfo Get(CurrencyIsoCode code)
		{
			return null;
		}
	}
}
