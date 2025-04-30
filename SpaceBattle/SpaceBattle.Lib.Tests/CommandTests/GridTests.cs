using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class GridTests : IDisposable
    {
        private readonly Dictionary<Vector, int[]> vectorCoordinates = new Dictionary<Vector, int[]>();
        private readonly object iocScope;

        public GridTests()
        {
            new InitCommand().Execute();
            iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Vector.GetX", (object[] args) =>
            {
                var vector = (Vector)args[0];
                if (!vectorCoordinates.ContainsKey(vector))
                {
                    throw new KeyNotFoundException("Vector not found in coordinates dictionary");
                }

                return (object)vectorCoordinates[vector][0];
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Vector.GetY", (object[] args) =>
            {
                var vector = (Vector)args[0];
                if (!vectorCoordinates.ContainsKey(vector))
                {
                    throw new KeyNotFoundException("Vector not found in coordinates dictionary");
                }

                return (object)vectorCoordinates[vector][1];
            }).Execute();
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }

        [Fact]
        public void Test1()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 15, 25 });
            vectorCoordinates[pos] = new int[] { 15, 25 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };

            grid.AddObject(obj, pos);

            Assert.True(grid.ContainsObjectInCell(obj, (1, 2)));
        }

        [Fact]
        public void Test2()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 15, 25 });
            vectorCoordinates[pos] = new int[] { 15, 25 };
            var obj1 = new Dictionary<string, object> { { "Id", "obj1" } };
            var obj2 = new Dictionary<string, object> { { "Id", "obj2" } };

            grid.AddObject(obj1, pos);
            grid.AddObject(obj2, pos);

            Assert.True(grid.ContainsObjectInCell(obj1, (1, 2)));
            Assert.True(grid.ContainsObjectInCell(obj2, (1, 2)));
        }

        [Fact]
        public void Test3()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 15, 25 });
            vectorCoordinates[pos] = new int[] { 15, 25 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };
            grid.AddObject(obj, pos);

            grid.RemoveObject(obj, pos);

            Assert.False(grid.ContainsObjectInCell(obj, (1, 2)));
        }

        [Fact]
        public void Test4()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 15, 25 });
            vectorCoordinates[pos] = new int[] { 15, 25 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };
            grid.AddObject(obj, pos);

            grid.RemoveObject(obj, pos);

            Assert.False(grid.ContainsObjectInCell(obj, (1, 2)));
        }

        [Fact]
        public void test5()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 15, 25 });
            vectorCoordinates[pos] = new int[] { 15, 25 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };

            grid.RemoveObject(obj, pos);
        }

        [Fact]
        public void Test6()
        {
            var grid = new Grid(10);
            var oldPos = new Vector(new int[] { 5, 5 });
            var newPos = new Vector(new int[] { 15, 15 });
            vectorCoordinates[oldPos] = new int[] { 5, 5 };
            vectorCoordinates[newPos] = new int[] { 15, 15 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };
            grid.AddObject(obj, oldPos);

            grid.UpdateObject(obj, oldPos, newPos);

            Assert.False(grid.ContainsObjectInCell(obj, (0, 0)));
            Assert.True(grid.ContainsObjectInCell(obj, (1, 1)));
        }

        [Fact]
        public void Test7()
        {
            var grid = new Grid(10);
            var oldPos = new Vector(new int[] { 15, 25 });
            var newPos = new Vector(new int[] { 16, 26 });
            vectorCoordinates[oldPos] = new int[] { 15, 25 };
            vectorCoordinates[newPos] = new int[] { 16, 26 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };
            grid.AddObject(obj, oldPos);

            grid.UpdateObject(obj, oldPos, newPos);

            Assert.True(grid.ContainsObjectInCell(obj, (1, 2)));
        }

        [Fact]
        public void Test8()
        {
            var grid = new Grid(10);
            var pos1 = new Vector(new int[] { 5, 5 });
            var pos2 = new Vector(new int[] { 15, 15 });
            var pos3 = new Vector(new int[] { 25, 25 });
            vectorCoordinates[pos1] = new int[] { 5, 5 };
            vectorCoordinates[pos2] = new int[] { 15, 15 };
            vectorCoordinates[pos3] = new int[] { 25, 25 };
            var obj1 = new Dictionary<string, object> { { "Id", "obj1" } };
            var obj2 = new Dictionary<string, object> { { "Id", "obj2" } };
            var obj3 = new Dictionary<string, object> { { "Id", "obj3" } };
            grid.AddObject(obj1, pos1);
            grid.AddObject(obj2, pos2);
            grid.AddObject(obj3, pos3);

            var nearby = grid.GetNearbyObjects(pos1).ToList();

            Assert.Contains(obj1, nearby);
            Assert.Contains(obj2, nearby);
            Assert.DoesNotContain(obj3, nearby);
        }

        [Fact]
        public void Test9()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 5, 5 });
            vectorCoordinates[pos] = new int[] { 5, 5 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };
            grid.AddObject(obj, pos);

            var nearby = grid.GetNearbyObjects(pos).ToList();

            Assert.Single(nearby);
            Assert.Contains(obj, nearby);
        }

        [Fact]
        public void Test10()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { 15, 25 });
            vectorCoordinates[pos] = new int[] { 15, 25 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };

            Assert.False(grid.ContainsObjectInCell(obj, (1, 2)));
        }

        [Fact]
        public void Test11()
        {
            var grid = new Grid(10);
            var pos = new Vector(new int[] { -5, -5 });
            vectorCoordinates[pos] = new int[] { -5, -5 };
            var obj = new Dictionary<string, object> { { "Id", "obj1" } };

            grid.AddObject(obj, pos);

            Assert.True(grid.ContainsObjectInCell(obj, (-1, -1)));
        }

        [Fact]
        public void GetNearbyObjects_DoesNotReturnObjectsOutsideAdjacentCells()
        {
            var grid = new Grid(10);
            var posCentral = new Vector(new int[] { 5, 5 });
            var posFar = new Vector(new int[] { 25, 25 });
            vectorCoordinates[posCentral] = new int[] { 5, 5 };
            vectorCoordinates[posFar] = new int[] { 25, 25 };
            var objCentral = new Dictionary<string, object> { { "Id", "objCentral" } };
            var objFar = new Dictionary<string, object> { { "Id", "objFar" } };
            grid.AddObject(objCentral, posCentral);
            grid.AddObject(objFar, posFar);

            var nearby = grid.GetNearbyObjects(posCentral).ToList();

            Assert.Contains(objCentral, nearby);
            Assert.DoesNotContain(objFar, nearby);
        }
    }
}
