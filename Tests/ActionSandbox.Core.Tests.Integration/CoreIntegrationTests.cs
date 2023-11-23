using ActionSandbox.Core;
using FluentAssertions;

namespace ActionSandbox.Core.Tests.Integration
{
    public class CoreIntegrationTests
    {
        [Fact]
        public void FakeTestFive()
        {
            Program.FakeMethodOne().Should().Be("Passed");
        }
        [Fact]
        public void FakeTestSix()
        {
            Assert.True(true);
        }
        [Fact]
        public void FakeTestSeven()
        {
            Assert.True(true);
        }
        [Fact]
        public void FakeTestEight()
        {
            Assert.True(true);
        }
    }
}