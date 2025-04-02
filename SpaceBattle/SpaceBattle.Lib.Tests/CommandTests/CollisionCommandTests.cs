using App;
using App.Scopes;
using SpaceBattle.Lib;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class CollisionCommandTests : IDisposable
    {
        public CollisionCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var collisionTree = new Dictionary<int, object>(){
                {1,new Dictionary<int, object>(){
                    {1,new Dictionary<int, object>(){
                        {0, new Dictionary<int, object>(){
                            {0, new Dictionary<int, object>()}
                        }}
                    }}
                }}
            };
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => collisionTree).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.GetPosition", (object[] args) =>
            {
                var obj = (IDictionary<string, object>)args[0];
                return (int[])obj["Position"];
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.GetVelocity", (object[] args) =>
            {
                var obj = (IDictionary<string, object>)args[0];
                return (int[])obj["Velocity"];
            }).Execute();


        }
        [Fact]
        public void CollisionCommandTrueTest()
        {
            var collisionCommand = new Mock<ICommand>();
            collisionCommand.Setup(c => c.Execute()).Verifiable("Collision!");

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionCommand.Object).Execute();
            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.SetupSet(obj => obj["Position"] = new int[] { 0, 0 });
            obj2.SetupSet(obj => obj["Position"] = new int[] { 1, 1 });

            obj1.SetupSet(obj => obj["Velocity"] = new int[] { 1, 1 });
            obj2.SetupSet(obj => obj["Velocity"] = new int[] { 1, 1 });

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            commandCollision.Execute();

            collisionCommand.VerifyAll();
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}