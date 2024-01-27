using Microsoft.EntityFrameworkCore;
using NMoneys.Serialization.EFCore;
using NMoneys.Serialization.Tests.EFCore.Support;
using NMoneys.Serialization.Tests.Support;

namespace NMoneys.Serialization.Tests.EFCore;

[TestFixture]
public class QuantityConverterTester
{
	private FileInfo? _dbFile;
	[OneTimeSetUp]
	public void SetupDb()
	{
		_dbFile = new FileInfo("QuantityPersistence.sdf");
	}

	[OneTimeTearDown]
	public void TearDownDb()
	{
		_dbFile?.Delete();
	}

	[Test]
	public void SaveEntity_SavesAsQuantityValue()
	{
		using var context = new QuantityDbContext(_dbFile!, new QuantityConverter(), new MoneyComparer());
		context.Database.EnsureCreated();

		var m = new Money(43.75m, CurrencyIsoCode.XTS);
		var id = new Guid(new string('1', 32));
		var model = new MoneyModel
		{
			Id = id,
			M = m,
			N = 42
		};
		context.Models.Add(model);
		context.SaveChanges();

		FormattableString sql = $"SELECT M FROM Models WHERE Id = {id.ToString()}";
		var queried = context.Database.SqlQuery<string>(sql)
			.ToArray()
			.Single();
		Assert.That(queried, Is.EqualTo("XTS 43.75"));
	}

	[Test]
	public void SaveEntity_AsQuantity_CanRoundtrip()
	{
		using var context = new QuantityDbContext(_dbFile!, new QuantityConverter(), new MoneyComparer());
		context.Database.EnsureCreated();

		var m = new Money(43.75m, CurrencyIsoCode.XTS);
		var id = new Guid(new string('2', 32));
		var model = new MoneyModel
		{
			Id = id,
			M = m,
			N = 42
		};
		context.Models.Add(model);
		context.SaveChanges();

		MoneyModel queried = context.Models.Single(mm => mm.Id == id);
		Assert.That(queried.M, Is.EqualTo(m));
	}

	[Test]
	public void SaveNullableEntity_SavesAsNull()
	{
		using var context = new QuantityDbContext(_dbFile!, new QuantityConverter(), new MoneyComparer());
		context.Database.EnsureCreated();

		var id = new Guid(new string('3', 32));
		var model = new NullableMoneyModel
		{
			Id = id,
			N = 42
		};
		context.NullableModels.Add(model);
		context.SaveChanges();

		FormattableString sql = $"SELECT M FROM NullableModels WHERE Id = {id.ToString()}";
		var queried = context.Database.SqlQuery<string?>(sql)
			.ToArray()
			.Single();
		Assert.That(queried, Is.Null);
	}

	[Test]
	public void SaveNullableEntity_AsQuantity_CanRoundtrip()
	{
		using var context = new QuantityDbContext(_dbFile!, new QuantityConverter(), new MoneyComparer());
		context.Database.EnsureCreated();

		var id = new Guid(new string('4', 32));
		var model = new NullableMoneyModel
		{
			Id = id,
			N = 42
		};
		context.NullableModels.Add(model);
		context.SaveChanges();

		NullableMoneyModel queried = context.NullableModels.Single(mm => mm.Id == id);
		Assert.That(queried.M, Is.Null);
	}
}
