namespace NMoneys.Tests.CustomConstraints
{
	internal static class Must
	{
		public class ContainEntryPoint
		{
			internal ContainEntryPoint() { }
		}

		public class NotContainEntryPoint
		{
			internal NotContainEntryPoint() { }
		}

		private static readonly ContainEntryPoint _containEntryPoint = new ContainEntryPoint();
		public static ContainEntryPoint Contain { get { return _containEntryPoint; } }

		public class HaveEntryPoint
		{
			internal HaveEntryPoint() { }
		}

		public class NotHaveEntryPoint
		{
			internal NotHaveEntryPoint() { }
		}

		private static readonly HaveEntryPoint _haveEntryPoint = new HaveEntryPoint();
		public static HaveEntryPoint Have { get { return _haveEntryPoint; } }

		public class BeEntryPoint
		{
			internal BeEntryPoint() { }
		}

		public class NotBeEntryPoint
		{
			internal NotBeEntryPoint() { }
		}

		private static readonly BeEntryPoint _beEntryPoint = new BeEntryPoint();
		public static BeEntryPoint Be { get { return _beEntryPoint; } }

		public class RaiseObsoleteEventEntryPoint { }

		public class NotRaiseObsoleteEventEntryPoint { }

		private static readonly RaiseObsoleteEventEntryPoint _raiseObsoleteEventEntryPoint = new RaiseObsoleteEventEntryPoint();
		public static RaiseObsoleteEventEntryPoint RaiseObsoleteEvent { get { return _raiseObsoleteEventEntryPoint; } }

		public class SatisfyEntryPoint
		{
			internal SatisfyEntryPoint() { }
		}

		public class NotSatisfyEntryPoint
		{
			internal NotSatisfyEntryPoint() { }
		}

		private static readonly SatisfyEntryPoint _satisfyEntryPoint = new SatisfyEntryPoint();
		public static SatisfyEntryPoint Satisfy { get { return _satisfyEntryPoint; } }

		public static class Not
		{
			private static readonly NotContainEntryPoint _containEntryPoint = new NotContainEntryPoint();
			public static NotContainEntryPoint Contain { get { return _containEntryPoint; } }

			private static readonly NotHaveEntryPoint _haveEntryPoint = new NotHaveEntryPoint();
			public static NotHaveEntryPoint Have { get { return _haveEntryPoint; } }

			private static readonly NotBeEntryPoint _beEntryPoint = new NotBeEntryPoint();
			public static NotBeEntryPoint Be { get { return _beEntryPoint; } }

			private static readonly NotSatisfyEntryPoint _satisfyEntryPoint = new NotSatisfyEntryPoint();
			public static NotSatisfyEntryPoint Satisfy { get { return _satisfyEntryPoint; } }

			private static readonly NotRaiseObsoleteEventEntryPoint _raiseObsoleteEventEntryPoint = new NotRaiseObsoleteEventEntryPoint();
			public static NotRaiseObsoleteEventEntryPoint Raise { get { return _raiseObsoleteEventEntryPoint; } }
		}
	}
}
