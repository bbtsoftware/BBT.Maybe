---
Order: 20
Title: Examples
Description: Example usage of BBT.Maybe
---

## Usage of factory methods

Maybe values are created using factory methods provided by Maybe. Methods with arguments for reference are available (`Maybe.Some<T>`).
For the none case parameterless methods can be used (`Maybe.None<T>`).
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

## Optional properties

Maybe is useful for optional properties. The default implementation of maybe represents the null case, therefore optional properties of type `Maybe<T>` are representing the null case too and must not be initialized.

```csharp
/// Data class with mandatory property Foo and optional property Bar. Default of Bar is null.
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

## Optional method arguments

Maybe can be decalred as optional method arguments using the default keyword.

```csharp
/// Data class with mandatory property A and optional property B. Default of B is null.
public void Foo(bool flag, Maybe<A> maybeA = default)
{
    ...
}
```

## Projection method

Maybe provides a projection method (Some method) with an argument of type Func<T, TProjected>.
The usage is similar to the null propagation operator.

```csharp
/// Class foo contains optional property bar.
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