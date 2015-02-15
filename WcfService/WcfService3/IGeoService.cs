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
        [WebInvoke(UriTemplate = "TripCountInTime/{epoch}", Method = "GET", ResponseFormat=WebMessageFormat.Json)]
        int GetTripCountInTime(string epoch);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "TripCount?l={l}&t={t}&r={r}&b={b}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        int GetTripCountInRegion(double l, double t, double r, double b);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "TripSum?l={l}&t={t}&r={r}&b={b}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        decimal GetTripSum(double l, double t, double r, double b);        

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UpdateTripData(TripData data);
    }
}
