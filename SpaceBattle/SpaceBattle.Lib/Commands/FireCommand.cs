using App;

namespace SpaceBattle.Lib
{
    public class FireCommand : ICommand
    {
        private readonly Vector _position;
        private readonly Vector _fireDirection;
        private readonly double _speed;
        private string? _lastWeaponId;
        public FireCommand(Vector position, Vector fireDirection, double speed = 1.0)
        {
            _position = position;
            _fireDirection = fireDirection;
            _speed = speed;
        }

        public void Execute()
        {
            _lastWeaponId = Guid.NewGuid().ToString();
            var weaponDict = Ioc.Resolve<IDictionary<string, object>>("Weapon.Create", _lastWeaponId);

            var weapon = Ioc.Resolve<IMoving>("Adapters.IMovingObject", weaponDict["Id"]);

            Ioc.Resolve<ICommand>("Weapon.Setup", weapon, _position, _fireDirection, _speed).Execute();

            Ioc.Resolve<ICommand>("Game.Item.Add", _lastWeaponId, weapon).Execute();

            var moveCommandWeapon = Ioc.Resolve<ICommand>("Commands.Move", weapon);
            var receiver = Ioc.Resolve<ICommandReceiver>("Game.Receiver");
            var startCommandWeapon = Ioc.Resolve<ICommand>("Actions.Start", new Dictionary<string, object>
            {
                { "Cmd", moveCommandWeapon}, { "Receiver", receiver},
                { "Name", "Move"}, { "Object", weaponDict}
            });
            startCommandWeapon.Execute();
        }

        public string? GetLastWeaponId() { return _lastWeaponId; }
    }
}
