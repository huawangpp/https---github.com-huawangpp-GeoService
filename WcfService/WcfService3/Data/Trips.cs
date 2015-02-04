using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Uber.Base;
using Uber.Core;

namespace Uber.Data
{        
    public class Trips
    {
        // trip indexed by trip id
        private Dictionary<ulong, Trip> trips = new Dictionary<ulong, Trip>();

        // trip count indexed by epoch
        private Dictionary<ulong, int> tripCountInTime = new Dictionary<ulong, int>();

        private TreeNode treeRoot = new TreeNode();

        private static Trips instance;

        private Trips() { }

        public static Trips Instance
        {
            get
            {
                if (Instance == null)
                {
                    instance = new Trips();
                }
                return instance;
            }
        }

        /// <summary>
        /// Contains the number of occuring trips indexed by time
        /// NOTE: In a real system this should be stored in a database, and may be maintained the service cache,
        /// each with a different retention policy
        /// </summary>
        public Dictionary<ulong, int> TripCountInTime
        {
            get
            {
                return this.tripCountInTime;
            }
        }

        /// <summary>
        /// Update the Trip data in the data store.
        /// NOTE: again in a real system, this need to deal with db/cache.
        /// </summary>
        /// <param name="epoch"></param>
        public void UpdateTrip(TripData data) 
        {
            // update the trip count based on time
            // lock it for multithreading issues 
            lock (this.TripCountInTime)
            {
                if (this.TripCountInTime.ContainsKey(data.epoch))
                {
                    this.TripCountInTime[data.epoch] += 1;
                }
                else
                {
                    this.TripCountInTime.Add(data.epoch, 1);
                }
            }

            // update the trip list
            this.updateTripList(data);
        }

        /// <summary>
        /// Get the Trip count for a particular time
        /// NOTE: again in a real system, this need to deal with db/cache.
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public int GetTripCount(ulong epoch)
        {
            // no support for delete so no need to lock it here.
            if (this.TripCountInTime.ContainsKey(epoch))
            {
                return this.TripCountInTime[epoch];
            }
            return 0;
        }

        private void updateTripList(TripData data)
        {
            Trip trip;
            if (this.trips.ContainsKey(data.tripId))
            {
                trip = this.trips[data.tripId];
            }
            else
            {
                trip = new Trip();
            }

            trip.Fare = data.fare;

            this.treeRoot.Insert(data, trip);
        }

    }
}