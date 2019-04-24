namespace Bbt.Maybe.Tests
{
    using Bbt.Maybe;
    using Bbt.Maybe.Tests.TestData;
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
                var lDerivedClass = new DerivedClass();
                var lMaybeDerivedClass = Maybe.Some(lDerivedClass);

                // Act
                var lMaybeBaseClass = lMaybeDerivedClass.Some<BaseClass>(aX => aX);

                // Assert
                lMaybeBaseClass.ShouldBe(Maybe.Some<BaseClass>(lDerivedClass));
            }

            [Fact]
            public void ProjectToReferencingClass_ShouldReturnMaybeReferencingClass()
            {
                // Arrange
                var lReferencedClass = new ReferencedClass();
                var lReferencingClass = new BaseClass();
                lReferencedClass.ReferencingClass = lReferencingClass;

                // Act
                var lMaybeReferencingClass = Maybe.Some(lReferencedClass).Some(aX => aX.ReferencingClass);

                // Assert
                lMaybeReferencingClass.ShouldBe(Maybe.Some<BaseClass>(lReferencingClass));
            }

            [Fact]
            public void None_ShouldReturnMaybeNone()
            {
                // Arrange
                var lMaybeReferencedClass = Maybe.None<ReferencedClass>();

                // Act
                var lMaybeReferencingClass = lMaybeReferencedClass.Some(aX => aX.ReferencingClass);

                // Assert
                lMaybeReferencingClass.ShouldBe(Maybe.None<BaseClass>());
            }
        }

        public sealed class TheDoMethod
        {
            [Fact]
            public void MaybeValueNotSet_ActionNotCalled()
            {
                // Arrange
                var lMaybeNone = Maybe.None<BaseClass>();
                var lCalled = false;

                // Act
                lMaybeNone.Do(aX => lCalled = true);

                // Assert
                lCalled.ShouldBeFalse();
            }

            [Fact]
            public void MaybeValueSet_ActionCalled()
            {
                // Arrange
                var lMaybeNone = Maybe.Some<BaseClass>(new BaseClass());
                var lCalled = false;

                // Act
                lMaybeNone.Do(aX => lCalled = true);

                // Assert
                lCalled.ShouldBeTrue();
            }
        }

        public sealed class TheDoIfNoneMethod
        {
            [Fact]
            public void MaybeValueNotSet_DoIfNoneActionCalled()
            {
                // Arrange
                var lMaybeNone = Maybe.None<BaseClass>();
                var lCalled = false;

                // Act
                lMaybeNone.Do(aX => { }).DoIfNone(() => lCalled = true);

                // Assert
                lCalled.ShouldBeTrue();
            }

            [Fact]
            public void MaybeValueSet_ActionNotCalled()
            {
                // Arrange
                var lMaybeNone = Maybe.Some<BaseClass>(new BaseClass());
                var lCalled = false;

                // Act
                lMaybeNone.Do(aX => { }).DoIfNone(() => lCalled = true);

                // Assert
                lCalled.ShouldBeFalse();
            }
        }

        public sealed class TheEqualsMethod
        {
            [Fact]
            public void BothNone_ReturnsTrue()
            {
                // Arrange
                var lMaybeNone = Maybe.None<BaseClass>();
                var lMaybeNone2 = Maybe.None<BaseClass>();

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothNoneOfDerivedType_ReturnsFalse()
            {
                // Arrange
                var lMaybeNone = Maybe.None<BaseClass>();
                var lMaybeNone2 = Maybe.None<DerivedClass>();

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeFalse();
            }

            [Fact]
            public void BothSameValue_ReturnsTrue()
            {
                // Arrange
                var lBaseClass = new BaseClass();
                var lMaybeNone = Maybe.Some(lBaseClass);
                var lMaybeNone2 = Maybe.Some(lBaseClass);

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothSameValueButDerivedType_ReturnsFalse()
            {
                // Arrange
                var lDerivedClass = new DerivedClass();
                var lMaybeNone = Maybe.Some<DerivedClass>(lDerivedClass);
                var lMaybeNone2 = Maybe.Some<BaseClass>(lDerivedClass);

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeFalse();
            }

            [Fact]
            public void BothNotSame_ReturnsFalse()
            {
                // Arrange
                var lClass = new BaseClass();
                var lClass2 = new BaseClass();
                var lMaybeNone = Maybe.Some(lClass);
                var lMaybeNone2 = Maybe.Some(lClass2);

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeFalse();
            }

            [Fact]
            public void NoneAndNotNone_ReturnsFalse()
            {
                // Arrange
                var lClass = new BaseClass();
                var lMaybeNone = Maybe.Some<BaseClass>(lClass);
                var lMaybeNone2 = Maybe.None<BaseClass>();

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeFalse();
            }
        }
    }
}
