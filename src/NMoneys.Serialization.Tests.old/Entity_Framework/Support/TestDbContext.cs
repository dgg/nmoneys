using System;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.IO;
using NMoneys.Serialization.Entity_Framework;

namespace NMoneys.Serialization.Tests.Entity_Framework.Support
{
	//[DbConfigurationType(typeof(TestDbConfiguration))]
	public class TestDbContext : DbContext
	{
		public TestDbContext(FileInfo dbFile) : base(connectionString(dbFile)) { }

		private static string connectionString(FileInfo dbFile)
		{
			var builder = new SqlCeConnectionStringBuilder
			{
				DataSource = dbFile.Name
			};
			return builder.ConnectionString;
		}

		public DbSet<TestEntity> Entities { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestEntity>().HasKey(e => e.Id);
			modelBuilder.ConfigureMonetaryQuantity();
		}

		public TestDbContext Add(Guid id, int number, string text, Money monetaryQuantity)
		{
			var entity = new TestEntity
			{
				Id = id,
				Number = number,
				Text = text,
				Quantity = new MonetaryQuantity(monetaryQuantity)
			};
			Entities.Add(entity);
			return this;
		}

	}
}