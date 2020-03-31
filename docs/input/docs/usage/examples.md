---
Order: 20
Title: Examples
Description: Example usage of BBT.Maybe
---

# Usage of factory methods

Maybe values are created using factory methods provided by Maybe. Methods with arguments for reference are available (`Maybe.Some<T>(T)`).
For the none case parameterless methods can be used (`Maybe.None<T>()`).
For nullable value types analogous methods postfixed with 'Struct' are provided.

```csharp
// Creation of a maybe for a declared reference type which could be instantiated or not.
var foo = new Foo();
var maybeFoo = Maybe.Some(foo);

// Creation of a maybe for a reference type which is null.
var maybeFoo2 = Maybe.None<Foo>();

// Creation of a maybe for a declared nullable value type which could be instantiated or not.
int? number = 5;
var maybeNumber = Maybe.SomeStruct<int>(number);

// Creation of a maybe for a nullable value type which is null.
var maybeNumber2 = Maybe.NoneStruct<int>();
```

# Maybe methods

Instead of direct access to a reference type variable, `Maybe<T>.Do(Action<T>)` is used to declare an action which is called in case the referenced value is instantiated. The `NoneCase.DoIfNone(Action)` method of its return value can be used to declare an action which is called if the referenced value is not instantiated.

```csharp
// Do action is only called if foo is instantiated, DoIfNone is called otherwise.
Maybe.Some(foo)
    .Do(x => x.Bar())
    .DoIfNone(() => Console.WriteLine("No value set"));
```

# Optional properties

Maybe is useful for optional properties. The default implementation of maybe represents the null case, therefore optional properties of type `Maybe<T>` are representing the null case too and must not be initialized.

```csharp
/// Data class with mandatory property Foo and optional property MaybeBar. Default of MaybeBar is Maybe.None<Bar>.
public class Data()
{
    public Data(Foo foo)
    {
        Foo = foo;
    }

    public Foo { get; }
    public Maybe<Bar> MaybeBar { get; set; }
}
```

# Optional method arguments

Maybe can be declared as optional method arguments using the default keyword.

```csharp
/// maybeBar is an optional parameter. Maybe.None<Bar>() is default.
public void Foo(Maybe<Bar> maybeBar = default)
{
    maybeBar.Do(x => x.Bar());
}
```

# Projection method

Maybe provides a projection method (`Maybe<T>.Some<TResult>(Func<T, TResult>)`) with an argument of type Func<T, TResult>.
The usage is similar to the null propagation operator.

```csharp
/// Class foo contains optional property Bar.
public class Foo()
{
    public Bar { get; set; } = null;
}
```

 Usage of the maybe projection functionality:

```csharp
Maybe.Some(foo).Some(x => x.Bar).Do(x => ...);
```

Maybe.Some(foo) is the factory method call for the creation of the maybe of type Foo.
The second call Some(x => x.Bar) is the projection method projecting the Foo maybe to a maybe of its property type Bar.
Therefore projections of cascades of properties can be done without the need of null checks or null propagation.