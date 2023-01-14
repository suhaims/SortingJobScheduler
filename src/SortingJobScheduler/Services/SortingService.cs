using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SortingJobScheduler.Interfaces.Services;

namespace SortingJobScheduler.Services
{
    public class SortingService: ISortingService
    {
        public SortingService() { }

        public IEnumerable<int> SortNumbers(IEnumerable<int> numbers)
        {
            var numbersList = new List<int>(numbers);
            numbersList.Sort();

            return numbersList;
        }
    }
}
