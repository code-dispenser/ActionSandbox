using FluentAssertions;

namespace ActionSandbox.Core.Tests.Unit
{
    public class CoreUnitTests
    {
        [Fact]
        public void FakeTestOne()
        {
            Program.FakeMethodTwo().Should().Be("Passed");
        }
        [Fact]
        public void FakeTestTwo()
        {
            Assert.True(true);
        }
        [Fact]
        public void FakeTestThree()
        {
            Assert.True(true);
        }
        [Fact]
        public void FakeTestFour()
        {
            Assert.True(true);
        }
    }
}