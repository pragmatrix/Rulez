using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rulez
{
	public class RuleSet : IDisposable
	{
		readonly List<IDisposable> _rules = new List<IDisposable>();

		public RuleSet()
		{
			var t = GetType();
			var methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (var m in methods)
			{
				if (m.GetCustomAttributes(typeof (RuleAttribute), false).Length == 0)
					continue;
				if (m.GetParameters().Length != 0)
					throw new Exception("Rules are now allowed to have parameters (yet).");
				var mCopy = m;
				addRule(() => mCopy.Invoke(this, null));
			}
		}

		public void Dispose()
		{
			for (int i = _rules.Count; i != 0; --i)
			{
				var r = _rules[i - 1];
				r.Dispose();
			}

			_rules.Clear();
		}

		public void addRule(Action rule)
		{
			_rules.Add(Rule.activate(rule));
		}
	}
}
