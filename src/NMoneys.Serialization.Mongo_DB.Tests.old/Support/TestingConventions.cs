using MongoDB.Bson.Serialization.Conventions;

namespace NMoneys.Serialization.Mongo_DB.Tests.Support
{
	public class TestingConventions
	{
		private static readonly string _name = "testing";
		private readonly ConventionPack _pack;

		public TestingConventions(params IConvention[] conventions)
		{
			_pack = new ConventionPack();
			_pack.AddRange(conventions);
		}

		public void Register()
		{
			ConventionRegistry.Register(_name, _pack, _ => true);
		}
	}
}