using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Uber.Base;

namespace Uber
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGeoService
    {
        [OperationContract]
        int GetTripCountInTime(ulong epoch);
        
        [OperationContract]
        int GetTripCountInRegion(Base.Region r);
        
        [OperationContract]
        double GetTripSum(Base.Region r);        

        [OperationContract]
        void UpdateTripData(TripData data);
    }
}
