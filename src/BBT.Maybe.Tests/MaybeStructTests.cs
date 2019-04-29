namespace BBT.Maybe.Tests
{
    using BBT.Maybe;
    using BBT.Maybe.Tests.TestData;
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
                var referencedStruct = default(ReferencedStruct);
                var referencingStruct = default(BaseStruct);
                referencedStruct.ReferencingStruct = referencingStruct;

                // Act
                var maybeReferencingStruct = Maybe.SomeStruct<ReferencedStruct>(referencedStruct)
                    .Some<BaseStruct>(x => x.ReferencingStruct);

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.SomeStruct<BaseStruct>(referencingStruct));
            }

            [Fact]
            public void None_ShouldReturnMaybeNone()
            {
                // Arrange
                var maybeReferencedStruct = Maybe.NoneStruct<ReferencedStruct>();

                // Act
                var maybeReferencingStruct = maybeReferencedStruct.Some<BaseStruct>(x => x.ReferencingStruct);

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.NoneStruct<BaseStruct>());
            }
        }

        public sealed class TheDoMethod
        {
            [Fact]
            public void MaybeValueNotSet_ActionNotCalled()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
                var called = false;

                // Act
                maybeNone.Do(x => called = true);

                // Assert
                called.ShouldBeFalse();
            }

            /// <summary>
            /// Test that <see cref="Maybe{T}.Do"/> does call delegate if maybe represents not-null case.
            /// </summary>
            [Fact]
            public void MaybeValueSet_ActionCalled()
            {
                // Arrange
                var maybeNone = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));
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
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
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
                var maybeNone = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));
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
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
                var maybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothSameValue_ReturnsTrue()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void NoneAndNotNone_ReturnsFalse()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }
        }
    }
}
