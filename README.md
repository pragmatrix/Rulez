Rulez is the simplest possible Rule engine I can imagine for .NET.

A `Rule` is specified by a .NET function (`Action`), that uses `Fact`s for its computation.

To specify a `Rule`, use

	Rule.activate(Action);

A `Fact` represents a mutable value. To create a fact, use either

	var integerValue = Fact.of(10);

or

	var myInt = Fact.of<int>();

to change a `Fact` assign a value to it:

	myInt.Value = 4;


A `Rule` is evaluated in the context of the current `Dispatcher`.

While a `Rule` is evaluated, all `Fact`s that are accessed are tracked. 

A `Rule` has a lifetime. When it is activated, it is automatically reevaluated as soon one of its referred `Fact`s change.

`Rule`s may change other `Fact`s.

For example, the following code keeps the `result` variable always at two times the `input` variable:

	var input = Fact.of(0);
	int output = 0;
	Rule.activate(() => output = input.Value*2);

Note that the evaluation is deferred through the .NET `Dispatcher`. This means that the output values don't change immediately when the input values are changed.
