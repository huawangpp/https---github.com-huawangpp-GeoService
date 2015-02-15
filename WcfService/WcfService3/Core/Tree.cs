using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Uber.Base;

namespace Uber.Core
{
    // Quad tree to index location of the trips
    public class TreeNode
    {
        public TreeNode()
        {
            // NOTE: set capacity to a small random number 3 for testing purpose
            this.Capacity = 3;
            this.TreeNodeData = new TreeNodeData();
        }

        public TreeNode NorthWest { get; private set; }
        public TreeNode NorthEast { get; private set; }
        public TreeNode SouthWest { get; private set; }
        public TreeNode SouthEast { get; private set; }

        public Region ContainedRegion { get; set; }

        public int Capacity { get; set; }

        public TreeNodeData TreeNodeData { get; private set; }

        public int Count { get; set; }

        // query based on region and return the list of tree nodes that intersects
        // with the region. 
        public List<TreeNode> QueryTreeNodesInRegion(Region r)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            this.traverseTree(r, nodes);

            return nodes;
        }

        private void traverseTree(Region r, List<TreeNode> nodes)
        {
            if (!this.ContainedRegion.Intersect(r))
            {
                return;
            }

            nodes.Add(this);

            if (this.NorthEast == null)
            {
                return;
            }

            this.NorthWest.traverseTree(r, nodes);
            this.NorthEast.traverseTree(r, nodes);
            this.SouthWest.traverseTree(r, nodes);
            this.SouthEast.traverseTree(r, nodes);
        }

        // Adding a trip to tree node based on the current location of the trip
        public bool Insert(TripData data, Trip trip)
        {
            if (!ContainedRegion.Contains(data.lat, data.lng))
            {
                return false;
            }

            if (!this.TreeNodeData.Trips.Contains(data.tripId))
            {
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

                if (this.NorthWest.Insert(data, trip)) return true;
                if (this.NorthEast.Insert(data, trip)) return true;
                if (this.SouthWest.Insert(data, trip)) return true;
                if (this.SouthEast.Insert(data, trip)) return true;
            }
            else
            {
                this.TreeNodeData.AddTrip(data);
            }

            return true;
        }

        // divide to subregions
        private void divide()
        {
            double x = (this.ContainedRegion.left + this.ContainedRegion.right) / 2.0;
            double y = (this.ContainedRegion.top + this.ContainedRegion.bottom) / 2.0;

            this.NorthWest = new TreeNode();
            this.NorthWest.ContainedRegion = new Region(this.ContainedRegion.left, this.ContainedRegion.top, x, y);

            this.NorthEast = new TreeNode();
            this.NorthEast.ContainedRegion = new Region(x, this.ContainedRegion.top, this.ContainedRegion.right, y);

            this.SouthWest = new TreeNode();
            this.SouthWest.ContainedRegion = new Region(this.ContainedRegion.left, y, x, this.ContainedRegion.bottom);

            this.SouthEast = new TreeNode();
            this.SouthEast.ContainedRegion = new Region(x, y, this.ContainedRegion.right, this.ContainedRegion.bottom);
        }

    }

    /// <summary>
    /// Contained trip data of a tree node.
    /// </summary>
    public class TreeNodeData
    {        
        private List<uint> startTrip = new List<uint>();
        private List<uint> endTrip = new List<uint>();

        private List<uint> trips = new List<uint>();

        public List<uint> Trips
        {
            get
            {
                return this.trips;
            }
        }

        public List<uint> StartTrips
        {
            get
            {
                return this.startTrip;
            }
        }

        public List<uint> EndTrips
        {
            get
            {
                return this.endTrip;
            }
        }

        public void AddTrip(TripData data)
        {
            if (data.eventType == "begin" && !startTrip.Contains(data.tripId))
            {
                startTrip.Add(data.tripId);
            }
            else if (data.eventType == "end" && !endTrip.Contains(data.tripId))
            {
                endTrip.Add(data.tripId);
            }

            if (!trips.Contains(data.tripId))
            {
                trips.Add(data.tripId);
            }
        }                
    }    
}