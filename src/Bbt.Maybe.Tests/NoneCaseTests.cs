namespace Bbt.Maybe.Tests
{
    using Bbt.Maybe;
    using Shouldly;
    using Xunit;

    public sealed class NoneCaseTests
    {
        public sealed class TheDoIfNoneMethod
        {
            [Fact]
            public void IsNotNoneCase_ISNotCalled()
            {
                // Arrange
                var lNoneCase = new NoneCase(false);
                var lCalled = false;

                // Act
                lNoneCase.DoIfNone(() => lCalled = true);

                // Assert
                lCalled.ShouldBeFalse();
            }

            [Fact]
            public void IsNotNoneCase_IsCalled()
            {
                // Arrange
                var lNoneCase = new NoneCase(true);
                var lCalled = false;

                // Act
                lNoneCase.DoIfNone(() => lCalled = true);

                // Assert
                lCalled.ShouldBeTrue();
            }
        }

        public sealed class TheEqualsMethod
        {
            [Fact]
            public void BothNoneCase_ReturnsTrue()
            {
                // Arrange
                var lNoneCase = new NoneCase(true);
                var lNoneCase2 = new NoneCase(true);

                // Act
                var lIsEqual = object.Equals(lNoneCase, lNoneCase2);

                // Assert
                lIsEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothNotNoneCase_ReturnsTrue()
            {
                // Arrange
                var lNoneCase = new NoneCase(false);
                var lNoneCase2 = new NoneCase(false);

                // Act
                var lIsEqual = object.Equals(lNoneCase, lNoneCase2);

                // Assert
                lIsEqual.ShouldBeTrue();
            }

            [Fact]
            public void NoneAndNotNoneCase_ReturnsFalse()
            {
                // Arrange
                var lNoneCase = new NoneCase(false);
                var lNoneCase2 = new NoneCase(true);

                // Act
                var lIsEqual = object.Equals(lNoneCase, lNoneCase2);

                // Assert
                lIsEqual.ShouldBeFalse();
            }
        }
    }
}
