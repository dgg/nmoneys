using System;
using System.IO;
using System.Linq;
using NMoneys.Extensions;
using NMoneys.Serialization.Tests.Entity_Framework.Support;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Serialization.Tests.Entity_Framework
{
	[TestFixture]
	public class PersistenceTester
	{
		private FileInfo _dbFile;
		[OneTimeSetUp]
		public void SetupDb()
		{
			_dbFile = new FileInfo("Persistence.sdf");
			_dbFile.Delete();
		}

		[OneTimeTearDown]
		public void TearDownDb()
		{
			_dbFile.Delete();
		}

		[Test]
		public void Roundtrip_IsPossible()
		{
			Guid oneKey = GuidBuilder.Build(1), twoKey = GuidBuilder.Build(2);

			using (var ctx = new TestDbContext(_dbFile))
			{
				ctx
					.Add(id: oneKey, number: 1, text: "uno", monetaryQuantity: 11.1m.Dkk())
					.Add(id: twoKey, number: 2, text: "dos", monetaryQuantity: 22.222m.Eur());

				ctx.SaveChanges();
			}

			using (var ctx = new TestDbContext(_dbFile))
			{
				TestEntity one = ctx.Entities.SingleOrDefault(e => e.Id == oneKey);

				Assert.That(one, Is.Not.Null);
				Assert.That(one, Must.Match.Expected(new
				{
					Number = 1,
					Quantity = new
					{
						Amount = 11.1m,
						Currency = CurrencyIsoCode.DKK.AlphabeticCode()
					}
				}));

				TestEntity two = ctx.Entities.SingleOrDefault(e => e.Id == twoKey);

				Assert.That(two, Is.Not.Null);
				Assert.That(two, Must.Match.Expected(new
				{
					Number = 2,
					Quantity = new
					{
						Amount = 22.222m,
						Currency = CurrencyIsoCode.EUR.AlphabeticCode()
					}
				}));
			}
		}
	}
}