namespace Bbt.Maybe.Tests
{
    using Bbt.Maybe;
    using Bbt.Maybe.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class MaybeStructTests
    {
        public sealed class TheSomeStructMethod
        {
            [Fact]
            public void ProjectToReferencingClass_ShouldReturnMaybeReferencingClass()
            {
                // Arrange
                var lReferencedStruct = default(ReferencedStruct);
                var lReferencingStruct = default(BaseStruct);
                lReferencedStruct.ReferencingStruct = lReferencingStruct;

                // Act
                var lMaybeReferencingStruct = Maybe.SomeStruct<ReferencedStruct>(lReferencedStruct)
                    .Some<BaseStruct>(aX => aX.ReferencingStruct);

                // Assert
                lMaybeReferencingStruct.ShouldBe(Maybe.SomeStruct<BaseStruct>(lReferencingStruct));
            }

            [Fact]
            public void None_ShouldReturnMaybeNone()
            {
                // Arrange
                var lMaybeReferencedStruct = Maybe.NoneStruct<ReferencedStruct>();

                // Act
                var lMaybeReferencingStruct = lMaybeReferencedStruct.Some<BaseStruct>(aX => aX.ReferencingStruct);

                // Assert
                lMaybeReferencingStruct.ShouldBe(Maybe.NoneStruct<BaseStruct>());
            }
        }

        public sealed class TheDoMethod
        {
            [Fact]
            public void MaybeValueNotSet_ActionNotCalled()
            {
                // Arrange
                var lMaybeNone = Maybe.NoneStruct<BaseStruct>();
                var lCalled = false;

                // Act
                lMaybeNone.Do(aX => lCalled = true);

                // Assert
                lCalled.ShouldBeFalse();
            }

            /// <summary>
            /// Test that <see cref="Maybe{T}.Do"/> does call delegate if maybe represents not-null case.
            /// </summary>
            [Fact]
            public void MaybeValueSet_ActionCalled()
            {
                // Arrange
                var lMaybeNone = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));
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
                var lMaybeNone = Maybe.NoneStruct<BaseStruct>();
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
                var lMaybeNone = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));
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
                var lMaybeNone = Maybe.NoneStruct<BaseStruct>();
                var lMaybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothSameValue_ReturnsTrue()
            {
                // Arrange
                var lBaseStruct = default(BaseStruct);
                var lMaybeNone = Maybe.SomeStruct<BaseStruct>(lBaseStruct);
                var lMaybeNone2 = Maybe.SomeStruct<BaseStruct>(lBaseStruct);

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeTrue();
            }

            [Fact]
            public void NoneAndNotNone_ReturnsFalse()
            {
                // Arrange
                var lStruct = default(BaseStruct);
                var lMaybeNone = Maybe.SomeStruct<BaseStruct>(lStruct);
                var lMaybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var lIsEqual = object.Equals(lMaybeNone, lMaybeNone2);

                // Assert
                lIsEqual.ShouldBeFalse();
            }
        }
    }
}
