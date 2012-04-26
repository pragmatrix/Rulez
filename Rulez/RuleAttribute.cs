using System;

namespace Rulez
{
	/*
		If a method is in a class derived from RuleSet and attributed 
		with the [Rule] attribute, it is automatically added to the RuleSet.
	*/

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class RuleAttribute : Attribute
	{
	}
}
