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
            var playerPermissions = (IEnumerable<string>)Ioc.Resolve<object>("Game.Item.Get", $"{_objectId}_permissions");

            var permission = playerPermissions.FirstOrDefault(perm => perm == $"{_playerId}:{_operation}")
            ?? throw new UnauthorizedAccessException($"Player {_playerId} is not authorized to perform operation '{_operation}' on object {_objectId}.");
        }
    }
}
