using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class GameRepositoryTests : IDisposable
    {
        public GameRepositoryTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Test_Add_Get_Item()
        {
            //Arrange
            var registerCmd = new RegisterIocDependencyGameRepository();
            var gameItem = new object();
            var id = "1257";

            //Act
            registerCmd.Execute();

            var cmdAdd = Ioc.Resolve<ICommand>("Game.Item.Add", id, gameItem);
            cmdAdd.Execute();
            var gameItemResult = Ioc.Resolve<object>("Game.Item.Get", id);

            //Assert
            Assert.Equal(gameItem, gameItemResult);
        }

        [Fact]
        public void Test_Add_Remove()
        {
            //Arrnage
            var registerCmd = new RegisterIocDependencyGameRepository();
            var gameItem = new object();
            var id = "12678";

            //Act
            registerCmd.Execute();

            var cmdAdd = Ioc.Resolve<ICommand>("Game.Item.Add", id, gameItem);
            cmdAdd.Execute();
            var cmdRemove = Ioc.Resolve<ICommand>("Game.Item.Remove", id);
            cmdRemove.Execute();

            //Assert
            Assert.Throws<KeyNotFoundException>(() => Ioc.Resolve<object>("Game.Item.Get", id));
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
