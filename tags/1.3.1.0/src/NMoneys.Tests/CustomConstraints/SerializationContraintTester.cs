using System;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace NMoneys.Tests.CustomConstraints
{
	[TestFixture]
	public class SerializationContraintTester
	{
		[Test]
		public void Match_SerializableType_True()
		{
			var serializable = new SerializableSubject("msg", new NotFiniteNumberException());
			var subject = new BinarySerializationConstraint<SerializableSubject>(Is.EqualTo);

			Assert.That(subject.Matches(serializable), Is.True);
		}

		[Test]
		public void Match_NonSerializable_False()
		{
			var nonSerializable = new FaultySerializationSubject(4);

			var subject = new BinarySerializationConstraint<FaultySerializationSubject>(Is.EqualTo);

			Assert.That(subject.Matches(nonSerializable), Is.False);
		}
	}

	[Serializable]
	internal class SerializableSubject : Exception, IEquatable<SerializableSubject>
	{
		public SerializableSubject() { }

		public SerializableSubject(string message) : base(message) { }

		public SerializableSubject(string message, Exception inner) : base(message, inner) { }

		protected SerializableSubject(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public bool Equals(SerializableSubject other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Message, Message) && other.InnerException.GetType().Equals(InnerException.GetType());
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (SerializableSubject)) return false;
			return Equals((SerializableSubject) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Message != null ? Message.GetHashCode() : 0)*397) ^ (InnerException!= null ? InnerException.GetHashCode() : 0);
			}
		}
	}

	[Serializable]
	internal class FaultySerializationSubject : IEquatable<FaultySerializationSubject>
	{
		public FaultySerializationSubject(int prop)
		{
			_prop = prop; 
		}

		[NonSerialized]
		private int _prop;
		public int Prop
		{
			get { return _prop; }
			set { _prop = value; }
		}

		public bool Equals(FaultySerializationSubject other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other._prop == _prop;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (FaultySerializationSubject)) return false;
			return Equals((FaultySerializationSubject) obj);
		}

		public override int GetHashCode()
		{
			return _prop;
		}
	}
}
