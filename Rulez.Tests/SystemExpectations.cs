using System;
using NUnit.Framework;

namespace Rulez.Tests
{
	[TestFixture]
	public class SystemExpectations
	{
		// adding an event receiver two times, will call it two times!

		[Test]
		public void duplicatedEventReceiver()
		{
			uint count = 0;

			Action myAction = () =>
				{
					++count;
				};

			var src = new Source();
			src.SourceEvent += myAction;
			src.SourceEvent += myAction;

			src.trigger();
			Assert.That(count, Is.EqualTo(2));

			src.SourceEvent -= myAction;
			src.trigger();
			Assert.That(count, Is.EqualTo(3));
		}

		sealed class Source
		{
			public event Action SourceEvent;

			public void trigger()
			{
				SourceEvent();
			}
		}



		[Test]
		public void duplicatedEventReceiverInClass()
		{
			_callCount = 0;

			var src = new Source();
			src.SourceEvent += receive;
			src.SourceEvent += receive;

			src.trigger();
			Assert.That(_callCount, Is.EqualTo(2));

			src.SourceEvent -= receive;
			src.trigger();

			Assert.That(_callCount, Is.EqualTo(3));
		}

		static uint _callCount;

		void receive()
		{
			++_callCount;
		}


	}
}
