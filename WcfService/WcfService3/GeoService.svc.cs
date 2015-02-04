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

        public int GetTripCountInTime(ulong epoch)
        {
            return this.trips.GetTripCount(epoch);
        }

        public int GetTripCountInRegion(Base.Region r)
        {
            return 0;
        }

        public double GetTripSum(Base.Region r)
        {
            return 0.0;
        }
    }
}
