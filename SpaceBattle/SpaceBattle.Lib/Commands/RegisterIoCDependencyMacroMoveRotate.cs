using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyMacroMoveRotate : ICommand
    {
        public void Execute()
        {
            var MacroMoveCmd = new CreateMacroCommandStrategy("Move");
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Macro.Move",
                (object[] args) => MacroMoveCmd.Resolve(args)
            ).Execute();

            var MacroRotateCmd = new CreateMacroCommandStrategy("Rotate");
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Macro.Rotate",
                (object[] args) => MacroRotateCmd.Resolve(args)
            ).Execute();
        }
    }
}
