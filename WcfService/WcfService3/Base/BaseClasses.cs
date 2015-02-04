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
    public struct Location
    {
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
        [DataMember]
        public Location topleft
        {
            get;
            set;
        }

        [DataMember]
        public Location bottomright
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
            return lat <= topleft.lat && lat >= bottomright.lat && lng >= topleft.lng && lng <= bottomright.lng;
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