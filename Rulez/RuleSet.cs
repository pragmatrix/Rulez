using System;
using System.Collections.Generic;

namespace Rulez
{
	public class RuleSet : IDisposable
	{
		readonly List<IDisposable> _rules = new List<IDisposable>();

		public void Dispose()
		{
			for (int i = _rules.Count; i != 0; --i)
			{
				var r = _rules[i - 1];
				r.Dispose();
			}

			_rules.Clear();
		}

		public void add(Action rule)
		{
			_rules.Add(Rule.activate(rule));
		}
	}
}
