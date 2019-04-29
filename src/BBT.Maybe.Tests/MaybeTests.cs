namespace BBT.Maybe.Tests
{
    using BBT.Maybe;
    using BBT.Maybe.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class MaybeTests
    {
        public sealed class TheSomeMethod
        {
            [Fact]
            public void DerivedCast_ShouldReturnMaybeBase()
            {
                // Arrange
                var derivedClass = new DerivedClass();
                var maybeDerivedClass = Maybe.Some(derivedClass);

                // Act
                var maybeBaseClass = maybeDerivedClass.Some<BaseClass>(x => x);

                // Assert
                maybeBaseClass.ShouldBe(Maybe.Some<BaseClass>(derivedClass));
            }

            [Fact]
            public void ProjectToReferencingClass_ShouldReturnMaybeReferencingClass()
            {
                // Arrange
                var referencedClass = new ReferencedClass();
                var referencingClass = new BaseClass();
                referencedClass.ReferencingClass = referencingClass;

                // Act
                var maybeReferencingClass = Maybe.Some(referencedClass).Some(x => x.ReferencingClass);

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.Some<BaseClass>(referencingClass));
            }

            [Fact]
            public void None_ShouldReturnMaybeNone()
            {
                // Arrange
                var maybeReferencedClass = Maybe.None<ReferencedClass>();

                // Act
                var maybeReferencingClass = maybeReferencedClass.Some(x => x.ReferencingClass);

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.None<BaseClass>());
            }
        }

        public sealed class TheDoMethod
        {
            [Fact]
            public void MaybeValueNotSet_ActionNotCalled()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();
                var called = false;

                // Act
                maybeNone.Do(x => called = true);

                // Assert
                called.ShouldBeFalse();
            }

            [Fact]
            public void MaybeValueSet_ActionCalled()
            {
                // Arrange
                var maybeNone = Maybe.Some<BaseClass>(new BaseClass());
                var called = false;

                // Act
                maybeNone.Do(x => called = true);

                // Assert
                called.ShouldBeTrue();
            }
        }

        public sealed class TheDoIfNoneMethod
        {
            [Fact]
            public void MaybeValueNotSet_DoIfNoneActionCalled()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();
                var called = false;

                // Act
                maybeNone.Do(x => { }).DoIfNone(() => called = true);

                // Assert
                called.ShouldBeTrue();
            }

            [Fact]
            public void MaybeValueSet_ActionNotCalled()
            {
                // Arrange
                var maybeNone = Maybe.Some<BaseClass>(new BaseClass());
                var called = false;

                // Act
                maybeNone.Do(x => { }).DoIfNone(() => called = true);

                // Assert
                called.ShouldBeFalse();
            }
        }

        public sealed class TheEqualsMethod
        {
            [Fact]
            public void BothNone_ReturnsTrue()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();
                var maybeNone2 = Maybe.None<BaseClass>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothNoneOfDerivedType_ReturnsFalse()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();
                var maybeNone2 = Maybe.None<DerivedClass>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void BothSameValue_ReturnsTrue()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some(baseClass);
                var maybeNone2 = Maybe.Some(baseClass);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothSameValueButDerivedType_ReturnsFalse()
            {
                // Arrange
                var derivedClass = new DerivedClass();
                var maybeNone = Maybe.Some<DerivedClass>(derivedClass);
                var maybeNone2 = Maybe.Some<BaseClass>(derivedClass);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void BothNotSame_ReturnsFalse()
            {
                // Arrange
                var baseClass = new BaseClass();
                var baseClass2 = new BaseClass();
                var maybeNone = Maybe.Some(baseClass);
                var maybeNone2 = Maybe.Some(baseClass2);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void NoneAndNotNone_ReturnsFalse()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some<BaseClass>(baseClass);
                var maybeNone2 = Maybe.None<BaseClass>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }
        }
    }
}
