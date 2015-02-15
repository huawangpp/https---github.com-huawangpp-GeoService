using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

using Uber.Base;

namespace ClientTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //GeoServiceReference.GeoServiceClient client = new GeoServiceReference.GeoServiceClient();
            //int r = client.GetTripCountInTime("1392864673070");

            // post
            var request1 = (HttpWebRequest)WebRequest.Create("http://localhost:1292/GeoService.svc/UpdateTripData");
            request1.Method = "POST";
            request1.ContentType = "text/json";

            var tripDataSerializer = new DataContractJsonSerializer(typeof(TripData));
            var tripData = new TripData {  
                eventType = "begin",
                tripId = 12345,
                lat = 10.0,
                lng = 11.0,
                fare = 9,
                epoch = 123456
            };

            string body;
            using (var memoryStream = new MemoryStream())
            using (var reader2 = new StreamReader(memoryStream))
            {
                tripDataSerializer.WriteObject(memoryStream, tripData);
                memoryStream.Position = 0; 
                body = reader2.ReadToEnd();
            }

            using (var streamWriter = new StreamWriter(request1.GetRequestStream()))
            {
                streamWriter.Write(body);
            }

            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;

            Stream stream1 = response1.GetResponseStream();
            StreamReader reader1 = new StreamReader(stream1);
            string r1 = reader1.ReadToEnd();

            // get
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:1292/GeoService.svc/TripCountInTime/123456");
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string r = reader.ReadToEnd();

        }
    }
}
