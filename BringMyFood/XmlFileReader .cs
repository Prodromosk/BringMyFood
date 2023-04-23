using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using System.IO;

namespace BringMyFood
{
    public class XmlFileReader
    {
        public Driver ReadDriverFromFile(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement driverElement = xmlDoc.Root.Element("Driver");

            if (driverElement == null)
            {
                throw new Exception("The XML file is not properly formatted. It should contain a 'Driver' element.");
            }

            Driver driver = new Driver
            {
                Name = driverElement.Attribute("name").Value
            };

            IEnumerable<XElement> journeyElements = driverElement.Elements("Journey");
            foreach (XElement journeyElement in journeyElements)
            {
                Journey journey = new Journey
                {
                    Date = DateTime.ParseExact(journeyElement.Attribute("date").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Collection = journeyElement.Element("Collection").Value,
                    Delivery = journeyElement.Element("Delivery").Value,
                    Distance = int.Parse(journeyElement.Element("Distance").Value)
                };

                driver.Journeys.Add(journey);
            }

            return driver;
        }
    }
}
