#Rulez

Rulez is the simplest possible Rule engine I can imagine for .NET.

##Facts and Rules

A *Rule* is specified by a .NET delegate (`Action`), that uses *Facts* for its computation.

To specify a *Rule*, use

	Rule.activate(Action);

A *Fact* represents a mutable value. To create a *Fact*, use either

	var integerValue = Fact.of(10);

or

	var myInt = Fact.of<int>();

To change a *Fact*, assign a value to it:

	myInt.Value = 4;


A *Rule* is evaluated in the context of the current `Dispatcher`.

While a *Rule* is evaluated, all *Facts* that are accessed are tracked. 

A *Rule* has a lifetime. When it is active, it is automatically reevaluated as soon one of its referred *Facts* change. To deactivate a *Rule* call `Dispose` on the return value of `Rule.activate()`.

*Rules* may change other *Facts*.

For example, the following code keeps the `result` variable always at two times the `input` variable:

	var input = Fact.of(0);
	int result = 0;
	Rule.activate(() => result = input.Value*2);

Note that the evaluation is deferred through the .NET `Dispatcher`. This means that the evaluation results may not be updated immediately.

##RuleSets

A *RuleSet* combines multiple *Rules* together.

To create a custom *RuleSet*, create a class that derives from *RuleSet*.

To add a *Rule* to your *RuleSet*, create a method that contains the *Rule* logic and attribute it with `[Rule]`.

Any `[Rule]` attributed method in a *RuleSet* derived class is automatically activated as a *Rule* when the *RuleSet* class is instantiated.

To deactivate the *RuleSet* call its `Dispose()` method.

TBD: Examples
