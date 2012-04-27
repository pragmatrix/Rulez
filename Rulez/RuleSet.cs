using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rulez
{
	public class RuleSet : IDisposable
	{
		readonly List<IDisposable> _rules = new List<IDisposable>();

		public RuleSet(Func<Type, object> resolver_ = null)
		{
			var t = GetType();
			var methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (var m in methods)
			{
				if (m.GetCustomAttributes(typeof (RuleAttribute), false).Length == 0)
					continue;
				addMethod(resolver_, m);
			}
		}

		void addMethod(Func<Type, object> resolver_, MethodInfo method)
		{
			// pre-resolve parameters.
			var parameters = method.GetParameters();
			if (resolver_ == null && parameters.Length != 0)
				throw new Exception("RuleSet needs a resolver to set up rules with non-empty parameter lists");

			var args = new object[parameters.Length];
			for (int i = 0; i != parameters.Length; ++i)
				args[i] = resolver_(parameters[i].ParameterType);

			var parameterExpressions = args.Select(Expression.Constant);
			var call = Expression.Call(Expression.Constant(this), method, parameterExpressions);
			addRule(Expression.Lambda<Action>(call).Compile());
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
