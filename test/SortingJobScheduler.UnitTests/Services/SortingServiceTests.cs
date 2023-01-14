using SortingJobScheduler.Services;
using SortingJobScheduler.Testing.Common.Attributes;

namespace SortingJobScheduler.UnitTests.Services
{
    public class SortingServiceTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void SortNumbers_EmptyInput_ShouldReturnEmptyArray(SortingService sut)
        {
            // Arrange
            var unsortedArray = new List<int>();

            // Act
            var result = sut.SortNumbers(unsortedArray);

            // Asset
            Assert.Empty(result);
        }

        [Theory]
        [AutoNSubstituteData]
        public void SortNumbers_UnsortedArrayInput_ShouldReturnSortedArray(SortingService sut)
        {
            // Arrange
            var unsortedArray = new List<int>() { 10, 5, 3, 8, 15, 9 };

            // Act
            var result = sut.SortNumbers(unsortedArray);

            // Asset
            Assert.Equal(unsortedArray.Count, result.Count());
            Assert.Equal(new List<int>() { 3, 5, 8, 9, 10, 15 }, result);
        }

        [Theory]
        [AutoNSubstituteData]
        public void SortNumbers_UnsortedArrayWithDuplicatesInput_ShouldReturnSortedArray(SortingService sut)
        {
            // Arrange
            var unsortedArray = new List<int>() { 10, 5, 3, 8, 15, 3, 5, 9 };

            // Act
            var result = sut.SortNumbers(unsortedArray);

            // Asset
            Assert.Equal(unsortedArray.Count, result.Count());
            Assert.Equal(new List<int>() { 3, 3, 5, 5, 8, 9, 10, 15 }, result);
        }
    }
}
