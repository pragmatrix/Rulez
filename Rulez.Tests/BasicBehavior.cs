using System.Windows.Threading;
using NUnit.Framework;

namespace Rulez.Tests
{
	[TestFixture]
	public class BasicBehavior
	{
		[Test]
		public void activation()
		{
			var f1 = Fact.of(10);
			var f2 = Fact.of<int>();

			Rule.activate(() =>
				{

					f2.Value = f1.Value*2;
					Dispatcher.ExitAllFrames();
				});

			Dispatcher.Run();

			Assert.That(f2 == 20);
		}


		[Test]
		public void changeTracking()
		{
			var f1 = Fact.of(10);

			int v2 = 0;

			Rule.activate(() =>
			{
				v2 = f1.Value * 2;
				Dispatcher.ExitAllFrames();
			});

			Dispatcher.Run();
			Assert.That(v2 == 20);

			f1.Value = 20;
			Dispatcher.Run();
			Assert.That(v2 == 40);
		}

		[Test]
		public void deactivation()
		{
			var f1 = Fact.of(10);

			int v2 = 0;
			int v3 = 0;

			var r1 = Rule.activate(() =>
			{
				v2 = f1.Value * 2;
			});

			Rule.activate(() =>
			{
				v3 = f1.Value * 2;
				Dispatcher.ExitAllFrames();
			});

			Dispatcher.Run();
			Assert.That(v2 == 20);
			Assert.That(v3 == 20);

			r1.Dispose();

			f1.Value = 20;
			Dispatcher.Run();
			Assert.That(v2 == 20);
			Assert.That(v3 == 40);
		}
	}
}
