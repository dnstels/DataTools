using System.Text;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit.Abstractions;

namespace JsonToolsTests
{
    public class RowsColumnsTests
    {
        private readonly ITestOutputHelper _outputConsole;
        public RowsColumnsTests(ITestOutputHelper output)
        {
            _outputConsole = output;
        }

        [Fact]
        public void InitAndCastArray_Test()
        {
            // Given
            int[,] array2 = new int[9, 5];

            InitAndShowArray(array2);
            CastAndShowArray(array2);

            // When
            var rowCount = array2.GetLength(0);
            var columnCount = array2.GetLength(1);
            
            // Then
            rowCount.Should().Be(9);
            columnCount.Should().Be(5);
        }

        private void CastAndShowArray(int[,] array2)
        {
            var ind = 0;
            var array1 = array2.Cast<int>().ToArray();
            foreach (var row in array1)
            {
                _outputConsole.WriteLine($"{ind++} {row}");
            }
        }

        private void InitAndShowArray(int[,] array2)
        {
            int val = 0;

            for (int i = 0; i < array2.GetLength(0); i++)
            {
                var buffSrt = new StringBuilder();
                for (int j = 0; j < array2.GetLength(1); j++)
                {
                    array2[i, j] = val++;
                    buffSrt.Append(array2[i, j] + "\t");
                }
                _outputConsole.WriteLine(buffSrt.ToString());
            }
        }

        [Theory]
        [InlineData(16, 1, 3)]
        [InlineData(28, 3, 5)]
        public void ColumnIndex_Test(int arrIndex, 
            int expColumnIndex, int expRowIndex)
        {
            // Given(Дано)
            int rowCount=9;
            var columnCount=5;
            
            // When(Когда)
            var columnIndex=arrIndex % columnCount;
            int rowIndex=arrIndex / columnCount;
        
            // Then(Тогда)
            columnIndex.Should().Be(expColumnIndex);
            rowIndex.Should().Be(expRowIndex);
        }

    }
}