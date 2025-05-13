using App;

namespace SpaceBattle.Lib;
public class SaveCollisionDataToFileCommand : ICommand
{
    private readonly string fileName;
    private readonly IList<int[]> vectors;

    public SaveCollisionDataToFileCommand(string fileName, IList<int[]> vectors)
    {
        this.fileName = fileName;
        this.vectors = vectors;
    }

    public void Execute()
    {
        var basePath = Ioc.Resolve<string>("Data.FilePath");
        var fullPath = Path.Combine(basePath, fileName);
        var lines = new List<string>();
        foreach (var vector in vectors)
        {
            lines.Add(string.Join(" ", vector));
        }

        File.WriteAllLines(fullPath, lines);
    }
}
