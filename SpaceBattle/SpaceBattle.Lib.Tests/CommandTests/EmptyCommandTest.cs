
namespace SpaceBattle.Lib.Tests
{
    public class EmptyCommandTests
    {

        [Fact]
        public void ShouldNotChangeState()
        {
            new EmptyCommand().Execute();
        }
    }
}
