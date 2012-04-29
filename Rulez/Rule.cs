using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using Rulez.Detail;

namespace Rulez
{
	public sealed class Rule : IDisposable
	{
		readonly Action _action;
		readonly Dispatcher _dispatcher;
		readonly List<Fact> _trackedFacts = new List<Fact>();
		bool _disposed;

		public static IDisposable activate(Action action)
		{
			var r = new Rule(action);
			r.schedule();
			return r;
		}

		internal Rule(Action action)
		{
			_action = action;
			_dispatcher = Dispatcher.CurrentDispatcher;
		}

		public void Dispose()
		{
			flushTrackedFacts();
			_disposed = true;
		}

		void schedule()
		{
			// no calls from different threads for now.
			Debug.Assert(Dispatcher.CurrentDispatcher == _dispatcher);

			// we want to flush tracked facts in advance so that multiple changes
			// result into only one call to schedule()
		
			flushTrackedFacts();
			_dispatcher.BeginInvoke((Action)evaluate, DispatcherPriority.Normal, null);
		}

		void evaluate()
		{
			// important: evaluate() is called from within the Dispatcher and so
			// we might have been disposed.

			if (_disposed)
				return;

			flushTrackedFacts();
			using (EvaluationFrame.push(this))
				_action();
		}

		internal void track(Fact fact)
		{
			Debug.Assert(EvaluationFrame.CurrentRule_ == this);
			fact.Changed += schedule;
			_trackedFacts.Add(fact);
		}

		void flushTrackedFacts()
		{
			_trackedFacts.ForEach(f => f.Changed -= schedule);
			_trackedFacts.Clear();
		}
	}
}
