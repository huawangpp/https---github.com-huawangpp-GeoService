using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using Uber.Base;
using Uber.Data;

namespace Uber
{
    public class GeoService : IGeoService
    {
        Trips trips = Trips.Instance;

        // Called by the established pub/sub channel
        public void UpdateTripData(TripData data)
        {
            // add to the trips indexed by time.
            this.trips.UpdateTrip(data);
        }

        /// <summary>
        /// Get the number of trips at a given time
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public int GetTripCountInTime(string epoch)
        {
            return this.trips.GetTripCount(epoch);
        }

        /// <summary>
        /// Get the number of trips that passed through a region
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int GetTripCountInRegion(double l, double t, double r, double b)
        {
            Region region = new Region(l, t, r, b);
            return this.trips.GetTripCountInRegion(region);
        }

        /// <summary>
        /// Get the sum of the begin/end trips in a region
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public decimal GetTripSum(double l, double t, double r, double b)
        {
            Region region = new Region(l, t, r, b);
            return this.trips.GetTripSum(region);
        }
    }
}
