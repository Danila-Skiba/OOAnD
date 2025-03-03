using App;

namespace SpaceBattle.Lib
{
    public class FireCommand : ICommand
    {
        private readonly IFireable _shooter;
        private string _lastWeaponId;
        public FireCommand(IFireable shooter)
        {
            _shooter = shooter;
        }

        public void Execute()
        {
            var weapon = Ioc.Resolve<IMoving>("Weapon.Create", _shooter.Position, _shooter.FireDirection);
            _lastWeaponId = Guid.NewGuid().ToString();
            Ioc.Resolve<ICommand>("Game.Item.Add", _lastWeaponId, weapon).Execute();
        }

        public string GetLastWeaponId() { return _lastWeaponId; }
    }
}
