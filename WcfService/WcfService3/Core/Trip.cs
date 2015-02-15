using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Uber.Base;

namespace Uber.Core
{
    public class Trip
    {        
        private List<Location> locations = new List<Location>();
        private Location start, end;

        public List<Location> Locations
        {
            get
            {
                return this.locations;
            }
        }

        public Location Start
        {
            get
            {
                return this.start;
            }
        }

        public Location End
        {
            get
            {
                return this.end;
            }
        }
        
        public decimal Fare
        {
            get;
            set;
        }

        public void AddToLocationList(double lat, double lng, string eventType)
        {
            Location l = new Location(lat, lng);
            locations.Add(l);

            if (eventType == "begin") this.start = l;
            else if (eventType == "end") this.end = l;
        }
    }
}