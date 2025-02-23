using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyRotateCommand : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Rotate",
                (object[] args) => new Rotate(Ioc.Resolve<IRotating>("Adapters.IRotatingObject", args[0]))).Execute();
        }
    }
}
