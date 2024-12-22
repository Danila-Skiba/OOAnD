
namespace SpaceBattle.Lib.Tests
{
    public class EmptyCommandTests
    {

        [Fact]
        public void Execute_ShouldNotChangeState()
        {
            new EmptyCommand().Execute(); 
        }
    }
}