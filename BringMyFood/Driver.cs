using System;
using System.Collections.Generic;

namespace BringMyFood
{
    public class Driver
    {
        public string Name { get; set; }
        public List<Journey> Journeys { get; set; }

        public Driver()
        {
            Journeys = new List<Journey>();
        }
    }
}
