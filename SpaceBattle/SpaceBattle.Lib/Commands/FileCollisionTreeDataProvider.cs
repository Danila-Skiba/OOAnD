namespace SpaceBattle.Lib
{
    public class FileCollisionTreeDataProvider : ICollisionTreeDataProvider
    {
        private readonly string filePath;
        public FileCollisionTreeDataProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public List<int[]> GetVectors()
        {
            return File.ReadLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray())
            .ToList();
        }
    }
}
