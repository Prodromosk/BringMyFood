using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace BringMyFood
{
    public class DataProcessor
    {
        private BlockingCollection<Driver> _driverQueue;

        public DataProcessor()
        {
            _driverQueue = new BlockingCollection<Driver>();
        }

        public void EnqueueDriver(Driver driver)
        {
            _driverQueue.Add(driver);
        }

        public List<Driver> GetDrivers()
        {
            return _driverQueue.OrderBy(driver => driver.Name).ToList();
        }

        public List<Journey> GetJourneysForDriver(string driverName)
        {
            Driver driver = _driverQueue.FirstOrDefault(d => d.Name == driverName);
            return driver?.Journeys.ToList() ?? new List<Journey>();
        }

        public Dictionary<string, int> GetTotalDistancesByDriver()
        {
            return _driverQueue.ToDictionary(driver => driver.Name, driver => driver.Journeys.Sum(j => j.Distance));
        }

        public Dictionary<DateTime, int> GetTotalDistancesByDate()
        {
            return _driverQueue.SelectMany(d => d.Journeys)
                               .GroupBy(j => j.Date.Date)
                               .ToDictionary(g => g.Key, g => g.Sum(j => j.Distance));
        }

        public KeyValuePair<Driver, Journey> GetLongestJourney()
        {
            Journey longestJourney = null;
            Driver longestJourneyDriver = null;

            foreach (Driver driver in _driverQueue)
            {
                foreach (Journey journey in driver.Journeys)
                {
                    if (longestJourney == null || journey.Distance > longestJourney.Distance)
                    {
                        longestJourney = journey;
                        longestJourneyDriver = driver;
                    }
                }
            }

            return new KeyValuePair<Driver, Journey>(longestJourneyDriver, longestJourney);
        }

    }
}
