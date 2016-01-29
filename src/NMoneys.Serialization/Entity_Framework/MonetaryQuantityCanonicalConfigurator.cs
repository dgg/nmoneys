using System.Data.Entity;

namespace NMoneys.Serialization.Entity_Framework
{
	public static class MonetaryQuantityCanonicalConfigurator
	{
		public static void Configure(DbModelBuilder modelBuilder)
		{
			modelBuilder.ComplexType<MonetaryQuantity>()
				.Property(c => c.Currency)
				.IsFixedLength().HasMaxLength(3);

			modelBuilder.ComplexType<MonetaryQuantity>()
				.Property(c => c.Amount).HasPrecision(19, 4);
		}

		public static void ConfigureMonetaryQuantity(this DbModelBuilder modelBuilder)
		{
			Configure(modelBuilder);
		}
	}
}