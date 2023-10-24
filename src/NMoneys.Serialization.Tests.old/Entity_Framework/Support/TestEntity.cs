using System;
using NMoneys.Serialization.Entity_Framework;

namespace NMoneys.Serialization.Tests.Entity_Framework.Support
{
	public class TestEntity
	{
		public Guid Id { get; set; }
		public string Text { get; set; }
		public int Number { get; set; }

		public MonetaryQuantity Quantity { get; set; }
	}
}