using App;

namespace SpaceBattle.Lib
{
    public class AuthCommand : ICommand
    {
        private readonly string _playerId;
        private readonly string _objectId;
        private readonly string _operation;

        public AuthCommand(string playerId, string objectId, string operation)
        {
            _playerId = playerId;
            _objectId = objectId;
            _operation = operation;
        }

        public void Execute()
        {
            var playerPermissions = Ioc.Resolve<IDictionary<string, string>>("Game.Item.Get", $"{_objectId}_permissions");
            var permissions = (IEnumerable<KeyValuePair<string, string>>)playerPermissions;
            _ = permissions.FirstOrDefault(e => e.Key == _playerId && e.Value == _operation).Value ?? throw new UnauthorizedAccessException($"Player {_playerId} is not authorized to perform operation '{_operation}' on object {_objectId}.");

        }
    }
}
