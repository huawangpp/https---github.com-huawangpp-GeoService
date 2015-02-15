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

        private Trips() 
        {
            //NOTE: set the range for testing purpose
            this.treeRoot.ContainedRegion = new Region(0, 0, 1000, 1000);
        }

        /// <summary>
        /// Singleton instance for trip management 
        /// </summary>
        public static Trips Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Trips();
                    // Pre load some fake trip data for testing purpose.
                    instance.PreLoadTrips();
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
        public int GetTripCount(string epoch)
        {
            ulong index = Convert.ToUInt64(epoch);

            // no support for delete so no need to lock it here.
            if (this.TripCountInTime.ContainsKey(index))
            {
                return this.TripCountInTime[index];
            }
            return 0;
        }

        /// <summary>
        /// Get the count of trips in a given region
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int GetTripCountInRegion(Region r)
        {
            int count = 0;
            List<TreeNode> nodes = this.treeRoot.QueryTreeNodesInRegion(r);
            List<uint> containedTrips = new List<uint>();

            foreach (TreeNode node in nodes) 
            {
                foreach (uint tripId in node.TreeNodeData.Trips)
                {
                    // check if the trip is already counted.
                    if (!containedTrips.Contains(tripId))
                    {
                        foreach (Location location in this.trips[tripId].Locations)
                        {
                            if (r.Contains(location.lat, location.lng))
                            {
                                count++;
                                containedTrips.Add(tripId);
                                break;
                            }
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Get the sum of trip in a given region
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public decimal GetTripSum(Region r)
        {
            decimal sum = 0;
            List<TreeNode> nodes = this.treeRoot.QueryTreeNodesInRegion(r);
            List<uint> containedTrips = new List<uint>();

            foreach (TreeNode node in nodes)
            {
                foreach (uint tripId in node.TreeNodeData.StartTrips)
                {
                    // check if the trip is already counted.
                    if (!containedTrips.Contains(tripId) && r.Contains(trips[tripId].Start.lat, trips[tripId].Start.lng))
                    {                        
                        sum += trips[tripId].Fare;
                        containedTrips.Add(tripId);                                                                           
                    }
                }

                foreach (uint tripId in node.TreeNodeData.EndTrips)
                {
                    // check if the trip is already counted.
                    if (!containedTrips.Contains(tripId) && r.Contains(trips[tripId].End.lat, trips[tripId].End.lng))
                    {
                        sum += trips[tripId].Fare;
                        containedTrips.Add(tripId);
                    }
                }
            }

            return sum;
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
                
                this.trips.Add(data.tripId, trip);
            }

            trip.Fare = data.fare;
            trip.AddToLocationList(data.lat, data.lng, data.eventType);

            this.treeRoot.Insert(data, trip);
        }

        // Pre load some fake trip data for testing purpose.
        private void PreLoadTrips()
        {
            // trip id: 1
            var tripData = new TripData()
            {
                eventType = "begin",
                tripId = 1,
                lat = 10.0,
                lng = 11.0,                
                epoch = 10000
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "update",
                tripId = 1,
                lat = 400.0,
                lng = 100.0,                
                epoch = 10001
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "end",
                tripId = 1,
                lat = 550.0,
                lng = 80.0,
                fare = 9,
                epoch = 10002
            };
            this.UpdateTrip(tripData);

            // trip id: 2
            tripData = new TripData()
            {
                eventType = "begin",
                tripId = 2,
                lat = 10.0,
                lng = 300.0,
                epoch = 10000
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "update",
                tripId = 2,
                lat = 350.0,
                lng = 320.0,
                epoch = 10001
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "end",
                tripId = 2,
                lat = 200.0,
                lng = 600.0,
                fare = 10,
                epoch = 10002
            };
            this.UpdateTrip(tripData);

            // trip id: 3
            tripData = new TripData()
            {
                eventType = "begin",
                tripId = 3,
                lat = 650.0,
                lng = 100.0,
                epoch = 10000
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "update",
                tripId = 3,
                lat = 670.0,
                lng = 330.0,
                epoch = 10001
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "end",
                tripId = 3,
                lat = 680.0,
                lng = 550.0,
                fare = 11,
                epoch = 10002
            };
            this.UpdateTrip(tripData);

            // trip id: 4
            tripData = new TripData()
            {
                eventType = "begin",
                tripId = 4,
                lat = 400.0,
                lng = 800.0,
                epoch = 10004
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "update",
                tripId = 4,
                lat = 490.0,
                lng = 690.0,
                epoch = 10005
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "end",
                tripId = 4,
                lat = 560.0,
                lng = 820.0,
                fare = 12,
                epoch = 10006
            };
            this.UpdateTrip(tripData);

            // trip id: 5
            tripData = new TripData()
            {
                eventType = "begin",
                tripId = 5,
                lat = 450.0,
                lng = 300.0,
                epoch = 10004
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "update",
                tripId = 5,
                lat = 560.0,
                lng = 560.0,
                epoch = 10005
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "end",
                tripId = 5,
                lat = 650.0,
                lng = 650.0,
                fare = 13,
                epoch = 10006
            };
            this.UpdateTrip(tripData);

            // trip id: 6
            tripData = new TripData()
            {
                eventType = "begin",
                tripId = 6,
                lat = 450.0,
                lng = 900.0,
                epoch = 10008
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "update",
                tripId = 6,
                lat = 600.0,
                lng = 720.0,
                epoch = 10009
            };
            this.UpdateTrip(tripData);

            tripData = new TripData()
            {
                eventType = "end",
                tripId = 6,
                lat = 900.0,
                lng = 400.0,
                fare = 14,
                epoch = 10010
            };
            this.UpdateTrip(tripData);
        }
    }
}