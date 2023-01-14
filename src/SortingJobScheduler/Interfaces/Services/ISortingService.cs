using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SortingJobScheduler.Interfaces.Services
{
    /// <summary>
    /// Service class to perform sorting operations
    /// </summary>
    public interface ISortingService
    {
        /// <summary>
        /// Sorts a sequence of numbers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns>Sorted list of numbers</returns>
        IEnumerable<int> SortNumbers(IEnumerable<int> numbers);
    }
}
