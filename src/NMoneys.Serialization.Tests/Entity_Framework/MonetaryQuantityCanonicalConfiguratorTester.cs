using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using NMoneys.Serialization.Tests.Entity_Framework.Support;
using NUnit.Framework;


namespace NMoneys.Serialization.Tests.Entity_Framework
{
	[TestFixture]
	public class MonetaryQuantityCanonicalConfiguratorTester
	{
		private FileInfo _dbFile;
		[OneTimeSetUp]
		public void SetupDb()
		{
			_dbFile = new FileInfo("Schema.sdf");
			_dbFile.Delete();

			var ctx = new TestDbContext(_dbFile);
			ctx.Database.CreateIfNotExists();
		}

		[OneTimeTearDown]
		public void TearDownDb()
		{
			_dbFile.Delete();
		}
		
		[Test]
		public void CurrencyColumn_NullableFixedWidth()
		{
			withColumnSchema("Currency", reader =>
			{
				Assert.That(reader["IsNullable"], Is.EqualTo("YES"));
				Assert.That(reader["DataType"], Is.EqualTo("nchar"));
				Assert.That(reader["Length"], Is.EqualTo(3));
			});
		}

		[Test]
		public void AmountColumn_NullablePreciseNumber()
		{
			withColumnSchema("Amount", reader =>
			{
				Assert.That(reader["IsNullable"], Is.EqualTo("YES"));
				Assert.That(reader["DataType"], Is.EqualTo("numeric"));
				Assert.That(reader["Precision"], Is.EqualTo(19));
				Assert.That(reader["Scale"], Is.EqualTo(4));
			});
		}

		private void withColumnSchema(string suffix, Action<SqlCeDataReader> assertions)
		{
			connect(cn =>
			{
				using (SqlCeCommand cmd = cn.CreateCommand())
				{
					cmd.CommandText = generateQuery(suffix);

					using (SqlCeDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
					{
						reader.Read(); // read the single row
						assertions(reader);

						reader.Close();
					}
				}
			});
		}

		private void connect(Action<SqlCeConnection> withConnection)
		{
			var builder = new SqlCeConnectionStringBuilder { DataSource = _dbFile.Name };
			using (SqlCeConnection connection = new SqlCeConnection(builder.ConnectionString))
			{
				connection.Open();
				withConnection(connection);

				connection.Close();
			}
		}

		private string generateQuery(string suffix)
		{
			return $@"
SELECT
	IS_NULLABLE AS IsNullable,
	DATA_TYPE AS DataType,
	CHARACTER_MAXIMUM_LENGTH AS Length,
	NUMERIC_PRECISION AS Precision,
	NUMERIC_SCALE AS Scale
FROM
	INFORMATION_SCHEMA.COLUMNS
WHERE
	TABLE_NAME = 'TestEntities' AND
	COLUMN_NAME Like '%_{suffix}'";
		}
	}
}