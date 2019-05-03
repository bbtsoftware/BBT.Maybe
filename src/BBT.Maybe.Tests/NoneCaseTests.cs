namespace BBT.Maybe.Tests
{
    using BBT.Maybe;
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
                var noneCase = new NoneCase(false);
                var called = false;

                // Act
                noneCase.DoIfNone(() => called = true);

                // Assert
                called.ShouldBeFalse();
            }

            [Fact]
            public void IsNotNoneCase_IsCalled()
            {
                // Arrange
                var noneCase = new NoneCase(true);
                var called = false;

                // Act
                noneCase.DoIfNone(() => called = true);

                // Assert
                called.ShouldBeTrue();
            }
        }

        public sealed class TheEqualsMethod
        {
            [Fact]
            public void BothNoneCase_ReturnsTrue()
            {
                // Arrange
                var noneCase = new NoneCase(true);
                var noneCase2 = new NoneCase(true);

                // Act
                var isEqual = object.Equals(noneCase, noneCase2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void BothNotNoneCase_ReturnsTrue()
            {
                // Arrange
                var noneCase = new NoneCase(false);
                var noneCase2 = new NoneCase(false);

                // Act
                var isEqual = object.Equals(noneCase, noneCase2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void NoneAndNotNoneCase_ReturnsFalse()
            {
                // Arrange
                var noneCase = new NoneCase(false);
                var noneCase2 = new NoneCase(true);

                // Act
                var isEqual = object.Equals(noneCase, noneCase2);

                // Assert
                isEqual.ShouldBeFalse();
            }
        }
    }
}
