using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NMoneys.Serialization.EFCore;
using NMoneys.Serialization.Tests.EFCore.Support;
using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Text_Json;
using Testing.Commons.Serialization;

using JsonConverter = NMoneys.Serialization.EFCore.JsonConverter;

namespace NMoneys.Serialization.Tests.EFCore;

[TestFixture]
public class JsonConverterTester
{
	private FileInfo? _dbFile;

	private JsonSerializerOptions JsonOptions { get; } = new()
	{
		Converters = { new MoneyConverter(), new JsonStringEnumConverter() },
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	[OneTimeSetUp]
	public void SetupDb()
	{
		_dbFile = new FileInfo("JsonPersistence.sdf");
	}

	[OneTimeTearDown]
	public void TearDownDb()
	{
		_dbFile?.Delete();
	}

	[Test, Explicit]
	public void SaveEntity_SavesAsQuantityValue()
	{
		using var context = new JsonDbContext(_dbFile!, new JsonConverter(JsonOptions), new MoneyComparer());
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

		FormattableString sql = $"SELECT M FROM Models WHERE ID = {id.ToString()}";
		var queried = context.Database.SqlQuery<string>(sql)
			.ToArray()
			.Single();
		Assert.That(queried, Is.EqualTo("{'amount':43.75,'currency':'XTS'}".Jsonify()));
	}

	[Test, Explicit]
	public void SaveEntity_AsQuantity_CanRoundtrip()
	{
		using var context = new JsonDbContext(_dbFile!, new JsonConverter(JsonOptions), new MoneyComparer());
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

	[Test, Explicit]
	public void SaveNullableEntity_SavesAsNull()
	{
		using var context = new JsonDbContext(_dbFile!, new JsonConverter(JsonOptions), new MoneyComparer());
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

	[Test, Explicit]
	public void SaveNullableEntity_AsQuantity_CanRoundtrip()
	{
		using var context = new JsonDbContext(_dbFile!, new JsonConverter(JsonOptions), new MoneyComparer());
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
