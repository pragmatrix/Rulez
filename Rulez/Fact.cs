using System;
using Rulez.Detail;

namespace Rulez
{
	public abstract class Fact
	{
		public static Fact<TypeT> of<TypeT>(TypeT value = default(TypeT))
		{
			return new Fact<TypeT>(value);
		}

		protected void trackAccessed()
		{
			var r = EvaluationFrame.CurrentRule_;
			if (r != null)
				r.track(this);
		}

		protected void trackChanged()
		{
			if (Changed != null)
				Changed();
		}

		internal event Action Changed;
	}

	public sealed class Fact<TypeT> : Fact
	{
		TypeT _value;

		internal Fact(TypeT initialValue)
		{
			_value = initialValue;
		}
		
		public TypeT Value
		{
			get
			{
				trackAccessed();
				return _value;
			} 
			set
			{
				if (Equals(_value, value))
					return;
				_value = value;
				trackChanged();
			}
		}

		public static implicit operator TypeT(Fact<TypeT> f)
		{
			return f.Value;
		}
	}

	public static class FactExtensions
	{
		public static void change<ValueT>(this Fact<ValueT> fact, Func<ValueT, ValueT> change)
		{
			fact.Value = change(fact.Value);
		}
	}
}
