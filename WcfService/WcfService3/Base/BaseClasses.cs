using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace Uber.Base
{
    [DataContract]
    public class Location
    {
        public Location() {}
        
        public Location(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        [DataMember]
        public double lat
        {
            get;
            set;
        }

        [DataMember]
        public double lng
        {
            get;
            set;
        }
    }

    [DataContract]
    public class Region
    {
        public Region() { }

        public Region(double left, double top, double right, double bottom)
        {
            this.top = top;
            this.left = left;
            this.bottom = bottom;
            this.right = right;
        }

        [DataMember]
        public double top
        {
            get;
            set;
        }

        [DataMember]
        public double left
        {
            get;
            set;
        }

        [DataMember]
        public double bottom
        {
            get;
            set;
        }

        [DataMember]
        public double right
        {
            get;
            set;
        }

        /// <summary>
        /// Check whether a location is inside of the region
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public bool Contains(double lat, double lng)
        {
            // NOTE: need to check whether how the calulation should be and whether projection is needed
            return lat <= right && lat >= left && lng >= top && lng <= bottom;
        }

        /// <summary>
        /// Check whether the region intersects
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool Intersect(Region r)
        {
            return !(r.left > this.right ||
                    r.right < this.left ||
                    r.top > this.bottom ||
                    r.bottom < this.top);
        }
    }

    [DataContract]
    public class TripData
    {
        private static DateTime epochBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [DataMember]
        public string eventType
        {
            get;
            set;
        }

        [DataMember]
        public uint tripId
        {
            get;
            set;
        }

        [DataMember]
        public double lat
        {
            get;
            set;
        }

        [DataMember]
        public double lng
        {
            get;
            set;
        }

        [DataMember]
        public decimal fare
        {
            get;
            set;
        }

        [DataMember]
        public ulong epoch
        {
            get;
            set;
        }

        public DateTime FromEpoch
        {
            get
            {                
                return epochBase.AddSeconds(epoch);
            }
        }
    }
}