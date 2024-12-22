namespace SpaceBattle.Lib.Tests
{
    public class VectorTests
    {
        [Fact]
        public void TestAddingThree_n()
        {
            var vector1 = new Vector(new int[] { 1, -1, 2 });
            var vector2 = new Vector(new int[] { -1, 1, -2 });

            var sum = vector1 + vector2;

            Assert.Equal(sum, new Vector(new int[] { 0, 0, 0 }));
        }

        [Fact]
        public void TestAddVectorsDifferentDimensions_1()
        {
            var vector1 = new Vector(new int[] { 1, 2, 3 });
            var vector2 = new Vector(new int[] { 1, 2 });

            Assert.Throws<InvalidOperationException>(() => { var sum = vector1 + vector2; });
        }

        [Fact]
        public void TestAddVectorsDifferentDimensions_2()
        {
            var vector1 = new Vector(new int[] { 1, 2 });
            var vector2 = new Vector(new int[] { 1, 2, 3 });

            Assert.Throws<InvalidOperationException>(() => { var sum = vector1 + vector2; });
        }

        [Fact]
        public void TestEqualityOfVectors()
        {
            var vector1 = new Vector(new int[] { 1, 2 });
            var vector2 = new Vector(new int[] { 1, 2 });

            Assert.Equal(vector1, vector2);
        }

        [Fact]
        public void TestOperatorOfVectors()
        {
            var vector1 = new Vector(new int[] { 1, 2 });
            var vector2 = new Vector(new int[] { 1, 2 });

            Assert.True(vector1 == vector2);
        }

        [Fact]
        public void TestNotEqualityOfVectors()
        {
            var vector1 = new Vector(new int[] { 1, 2 });
            var vector2 = new Vector(new int[] { 0, 0 });

            Assert.NotEqual(vector1, vector2);
        }

        [Fact]
        public void TestNotOperatorOfVectors()
        {
            var vector1 = new Vector(new int[] { 1, 2 });
            var vector2 = new Vector(new int[] { 0, 0 });

            Assert.True(vector1 != vector2);
        }

        [Fact]
        public void TestHashCode()
        {
            var coordinates = new int[] { 1, 2 };
            var vector1 = new Vector(coordinates);
            var vector2 = new Vector(coordinates);

            Assert.Equal(vector1.GetHashCode(), vector2.GetHashCode());
        }

        [Fact]
        public void TestAnotherTypeAndVector()
        {
            var vector1 = new Vector(new int[] { 1, 2 });
            var vector2 = new object();

            Assert.False(vector1.Equals(vector2));
        }
    }
}
