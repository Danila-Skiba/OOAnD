using App;

namespace SpaceBattle.Lib
{
    public class CollisionDataPreparingCommand : ICommand
    {
        private readonly ICollisionDataGenerator generator;
        public CollisionDataPreparingCommand(ICollisionDataGenerator generator)
        {
            this.generator = generator;
        }

        public void Execute()
        {
            var vectors = generator.GenerateCollisionData();
            var fileName = $"{generator.object1}_{generator.object2}.txt";
            var saveCommand = Ioc.Resolve<ICommand>("Collision.SaveDataToFile", fileName, vectors);
            saveCommand.Execute();
        }
    }
}
