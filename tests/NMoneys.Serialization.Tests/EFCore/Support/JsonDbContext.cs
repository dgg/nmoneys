using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NMoneys.Serialization.EFCore;
using NMoneys.Serialization.Tests.Support;

namespace NMoneys.Serialization.Tests.EFCore.Support;

public class JsonDbContext : DbContext
{
	private readonly ValueComparer<Money> _comparer;
	private readonly string _connectionString;
	private readonly ValueConverter<Money, string> _converter;
		public JsonDbContext(FileInfo dbFile, JsonConverter converter, ValueComparer<Money> comparer)
	{
		_comparer = comparer;
		_converter = converter;
		var builder = new SqliteConnectionStringBuilder
		{
			DataSource = dbFile.Name
		};
		_connectionString = builder.ConnectionString;
	}
	public DbSet<MoneyModel> Models { get; set; }
	public DbSet<NullableMoneyModel> NullableModels { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		base.OnConfiguring(options);

		options.UseSqlite(_connectionString);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<MoneyModel>().HasKey(e => e.Id);
		builder.Entity<NullableMoneyModel>().HasKey(e => e.Id);
		builder.Entity<MoneyModel>().Property(e => e.M).HasConversion(_converter, _comparer);
		builder.Entity<NullableMoneyModel>().Property(e => e.M).HasConversion(_converter, _comparer);
		base.OnModelCreating(builder);
	}
}
