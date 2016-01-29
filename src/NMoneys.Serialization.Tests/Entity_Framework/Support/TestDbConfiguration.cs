using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServerCompact;

namespace NMoneys.Serialization.Tests.Entity_Framework.Support
{
	public class TestDbConfiguration : DbConfiguration
	{
		public TestDbConfiguration()
		{
			string providerName = "System.Data.SqlServerCe.4.0";
			SetProviderServices(providerName, SqlCeProviderServices.Instance);
			SetDefaultConnectionFactory(new SqlCeConnectionFactory(providerName));
		}
	}
}