using System;
using System.Collections.Generic;
using System.Diagnostics;
using Toolbox;

namespace Rulez.Detail
{
	sealed class EvaluationFrame
	{
		readonly Stack<Rule> _ruleStack = new Stack<Rule>();

		public static IDisposable push(Rule rule)
		{
			return ForThisThread.pushRule(rule);
		}

		public static Rule CurrentRule_
		{
			get { return ForThisThread.tryGetCurrent(); }
		}

		IDisposable pushRule(Rule rule)
		{
			_ruleStack.Push(rule);

			return new DisposeAction(() =>
				{
					var r = _ruleStack.Pop();
					Debug.Assert(r == rule);
				});
		}

		Rule tryGetCurrent()
		{
			return _ruleStack.Count == 0 
				? null 
				: _ruleStack.Peek();
		}

		static EvaluationFrame ForThisThread
		{
			get { return _frame ?? (_frame = new EvaluationFrame()); }
		}

		[ThreadStatic]
		static EvaluationFrame _frame;
	}
}
