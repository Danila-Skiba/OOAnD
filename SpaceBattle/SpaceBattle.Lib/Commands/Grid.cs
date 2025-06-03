using App;

namespace SpaceBattle.Lib
{
    public class Grid
    {
        private readonly int cellSize;
        private readonly Dictionary<(int, int), List<IDictionary<string, object>>> cells;

        public Grid(int cellSize)
        {
            this.cellSize = cellSize;
            cells = new Dictionary<(int, int), List<IDictionary<string, object>>>();
        }

        private (int, int) GetCell(Vector position)
        {
            var x = Ioc.Resolve<int>("Vector.GetX", position);
            var y = Ioc.Resolve<int>("Vector.GetY", position);
            var dx = (double)x / cellSize;
            var dy = (double)y / cellSize;
            var cellX = (int)Math.Floor(dx);
            var cellY = (int)Math.Floor(dy);
            return (cellX, cellY);
        }

        public void AddObject(IDictionary<string, object> obj, Vector position)
        {
            var cell = GetCell(position);
            if (!cells.ContainsKey(cell))
            {
                cells[cell] = new List<IDictionary<string, object>>();
            }

            cells[cell].Add(obj);
        }

        public void RemoveObject(IDictionary<string, object> obj, Vector position)
        {
            var cell = GetCell(position);
            if (cells.ContainsKey(cell))
            {
                cells[cell].Remove(obj);
                if (cells[cell].Count == 0)
                {
                    cells.Remove(cell);
                }
            }
        }

        public void UpdateObject(IDictionary<string, object> obj, Vector oldPosition, Vector newPosition)
        {
            RemoveObject(obj, oldPosition);
            AddObject(obj, newPosition);
        }

        public IEnumerable<IDictionary<string, object>> GetNearbyObjects(Vector position)
        {
            var centerCell = GetCell(position);
            return Enumerable.Range(-1, 3)
                .SelectMany(dx => Enumerable.Range(-1, 3), (dx, dy) => (dx, dy))
                .Select(offset => (centerCell.Item1 + offset.dx, centerCell.Item2 + offset.dy))
                .Where(cell => cells.ContainsKey(cell))
                .SelectMany(cell => cells[cell]);
        }

        public bool ContainsObjectInCell(IDictionary<string, object> obj, (int, int) cell)
        {
            return cells.TryGetValue(cell, out var list) && list.Contains(obj);
        }
    }
}
