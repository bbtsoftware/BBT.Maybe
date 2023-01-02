# Core principles

C# does not (yet) include a language construct for null safe treatment of optional reference-type variables.
Therefore every method argument, return value, class member, property and local variable is a challenge, because a value may or may not be assigned.
A possible solution is implementing a null condition for each reference call with an if clause.
This contradicts the fail fast approach, makes code less readable and increases cyclomatic complexity.
A better approach is to introduce a functional option type.

## Advantages

* Explicit method signature: Declaration of optional reference type argument and return value through a typified construct make method documentation redundant.
* Prevention of null reference exceptions: Straight access to the nullable reference value is not possible, an action for the not-null case is called instead.

## Implementation details

* Maybe is a value type
* Default of maybe is the null case
* Maybe supports actions for handling the null and the not null case
* Maybe is serializable
* Provides factory methods for maybe
