using Microsoft.VisualStudio.TestTools.UnitTesting;
using BringMyFood;
using System.Linq;

namespace BringMyFood
{
    [TestClass]
    public class DataProcessorTests
    {
        [TestMethod]
        public void EnqueueDriver_AddDriver_DriverIsAdded()
        {
            // Arrange
            DataProcessor dataProcessor = new DataProcessor();
            Driver driver = new Driver { Name = "John Doe" };

            // Act
            dataProcessor.EnqueueDriver(driver);

            // Assert
            var drivers = dataProcessor.GetDrivers();
            Assert.AreEqual(1, drivers.Count);
            Assert.AreEqual("John Doe", drivers.First().Name);
        }

        [TestMethod]
        public void GetDrivers_AddMultipleDrivers_ReturnsSortedDrivers()
        {
            // Arrange
            DataProcessor dataProcessor = new DataProcessor();
            Driver driver1 = new Driver { Name = "Alice" };
            Driver driver2 = new Driver { Name = "Bob" };
            Driver driver3 = new Driver { Name = "Charlie" };

            dataProcessor.EnqueueDriver(driver1);
            dataProcessor.EnqueueDriver(driver2);
            dataProcessor.EnqueueDriver(driver3);

            // Act
            var drivers = dataProcessor.GetDrivers();

            // Assert
            Assert.AreEqual(3, drivers.Count);
            Assert.AreEqual("Alice", drivers[0].Name);
            Assert.AreEqual("Bob", drivers[1].Name);
            Assert.AreEqual("Charlie", drivers[2].Name);
        }
    }

}


