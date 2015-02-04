using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Uber.Base;

namespace Uber.Core
{
    public class TreeNode
    {
        public TreeNode NorthWest { get; set; }
        public TreeNode NorthEast { get; set; }
        public TreeNode SouthWest { get; set; }
        public TreeNode SouthEast { get; set; }

        public Region ContainedRegion { get; set; }

        public int Capacity { get; set; }

        public TreeNodeData TreeNodeData { get; set; }

        public int Count { get; set; }

        public bool Insert(TripData data, Trip trip)
        {
            if (!ContainedRegion.Contains(data.lat, data.lng))
            {
                return false;
            }

            if (this.Count < this.Capacity)
            {
                this.TreeNodeData.AddTrip(data);
                this.Count += 1;
                return true;
            }

            if (this.NorthWest == null)
            {
                this.divide();
            }

            // TODO: finish add to the children

            return false;
        }

        private void divide()
        {
            this.NorthWest = new TreeNode();
        }

    }

    public class TreeNodeData
    {        
        private List<uint> startTrip = new List<uint>();
        private List<uint> endTrip = new List<uint>();

        private List<uint> trips = new List<uint>();        

        private int locationListIndex = -1;

        public void AddTrip(TripData data)
        {
            if (data.eventType == "begin")
            {
                startTrip.Add(data.tripId);
            }
            else if (data.eventType == "end")
            {
                endTrip.Add(data.tripId);
            }

            //TODO: manage the trips
            if (trips.Contains(data.tripId))
            {
            }

            trips.Add(data.tripId);
        }                
    }

    // Quad tree to index location of the trips
    public class QuadTree
    {
        public void Insert(TripData data, Trip trip)
        {            
        }
    }
}