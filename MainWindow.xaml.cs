using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Device.Location;
using System.Net.Http;
using System.Xml;
using System.Globalization;

namespace WpfApp1FrameWorkBing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<PointsInformation> listofMaps = retrieveOffices();

            foreach (var item in listofMaps)
            {
                AddPoint(new Location(item.Latitude, item.Longitude) , item.OfficeName);
            }


        }
        private void AddPoint(Location location, string label)
        {
            Pushpin pin = new Pushpin();
            pin.Location = location;

            MapLayer.SetPosition(pin, location);

            myMap.Children.Add(pin);


            TextBlock textBlock = new TextBlock();
            textBlock.Text = label;

            MapLayer.SetPosition(textBlock, location);

            myMap.Children.Add(textBlock);
        }
        private double CalculateDistance(Location point1, Location point2)
        {
            GeoCoordinate coord1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
            GeoCoordinate coord2 = new GeoCoordinate(point2.Latitude, point2.Longitude);

            return coord1.GetDistanceTo(coord2) / 1000; // Distance in kilometers
        }
        private void myMap_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the clicked location
            Point mousePosition = e.GetPosition(myMap);
            Location clickedLocation = myMap.ViewportPointToLocation(mousePosition);


            List<PointsInformation> listofpoints = new List<PointsInformation>();

           
            // For demonstration, calculate the distance between the clicked point and the sample point
            for (int i = 0; myMap.Children.Count > i; i++)
            {
                Location samplePoint = myMap.Children[i].ReadLocalValue(MapLayer.PositionProperty) as Location;
                
                if (samplePoint?.Latitude != null)
                {
                    PointsInformation point = new PointsInformation();
                    point.DistanceKM = CalculateDistance(clickedLocation, samplePoint);

                    // Assuming i is the index of the Pushpin in myMap.Children
                    Pushpin pushpin = myMap.Children[i].ReadLocalValue(MapLayer.PositionProperty);

                    if (pushpin != null)
                    {
                        // Retrieve the label from the Tag property
                        string label = pushpin.Tag as string;

                        if (label != null)
                        {
                            // Now you can use the label variable
                            point.OfficeName = label;
                        }
                    }

                    listofpoints.Add(point); 
                }
            }
            

            MessageBox.Show($"Distance from closest office: {listofpoints.OrderBy(num => Math.Abs(num.DistanceKM)).Select(num => num.DistanceKM).First()} kilometers; {listofpoints.OrderBy(num => Math.Abs(num.DistanceKM)).Select(num => num.OfficeName).First()}"+
                " "+
           $"Distance from farest office: {listofpoints.OrderByDescending(num => Math.Abs(num.DistanceKM)).Select(num => num.DistanceKM).First()} kilometers; {listofpoints.OrderByDescending(num => Math.Abs(num.DistanceKM)).Select(num => num.OfficeName).First()}");
        }
        private static List<PointsInformation> retrieveOffices()
        {
            List<PointsInformation> listofMaps = new List<PointsInformation>();

            List<string> list = new List<string>
            {
                "Altamonte Springs, FL, USA",
                "Apopka, FL, USA",
                "Fort Lauderdale, FL, USA",
                "Jacksonville, FL, USA",
                "Orlando, FL, USA",
                "Tampa, FL, USA",
                "Addison, TX, USA",
                "Austin, TX, USA",
                "Dallas, TX, USA",
                "El Paso, TX, USA",
                "Fort Worth, TX, USA",
                "Houston, TX, USA",
                "Pasadena, TX, USA",
                "Plano, TX, USA",
                "San Antonio, TX, USA",
                "Waco, TX, USA",
                "Chico, CA, USA",
                "El Dorado Hills, CA, USA",
                "Rancho Cordova, CA, USA",
                "San Diego, CA, USA",
                "San Francisco Bay Area, CA, USA",
                "San Francisco, CA, USA",
                "San Jose, CA, USA",
                "Colorado Springs, CO, USA",
                "Denver, CO, USA",
                "Englewood, CO, USA",
                "Cincinnati, OH, USA",
                "Cleveland, OH, USA",
                "Columbus, OH, USA",
                "Indianapolis, IN, USA",
                "Warsaw, IN, USA",
                "Allentown, PA, USA",
                "Erie, PA, USA",
                "Pittsburgh, PA, USA",
                "Reston, VA, USA",
                "Richmond, VA, USA",
                "Charlotte, NC, USA",
                "Wilson, NC, USA",
                "Minneapolis, MN, USA",
                "Richfield, MN, USA",
                "Kansas City, MO, USA",
                "St Louis, MO, USA",
                "Greenville, SC, USA",
                "Moncks Corner, SC, USA",
                "Chicago, IL, USA",
                "Tuscaloosa, AL, USA",
                "Vance, AL, USA",
                "Woodbridge, NJ, USA",
                "Nashville, TN, USA",
                "Baltimore, MD, USA",
                "Louisville, KY, USA",
                "Atlanta, GA, USA",
                "Boise, ID, USA",
                "Chandler, AZ, USA",
                "Phoenix, AZ, USA",
                "Overland Park, KS, USA",
                "Omaha, NE, USA",
                "Council Bluffs, IA, USA",
                "Burlington, MA, USA",
                "Milwaukee, WI, USA,"


            };

            foreach (var item in list)
            {

                var accentMapping = new Dictionary<char, char>
                {
                    {'á', 'a'},
                    {'é', 'e'},
                    {'í', 'i'},
                    {'ó', 'o'},
                    {'ú', 'u'}
                };


                using (HttpClient client = new HttpClient())
                {
                    Console.WriteLine($"http://dev.virtualearth.net/REST/v1/Locations/US/{item}?o=xml&key=Aowl3fmRvC-iCiFMh_IIb3kG7UAHkcLy8maARHJQS56S4owWmfwxPkJTn8Nt4sTq");

                    var request = new HttpRequestMessage(HttpMethod.Get, $"http://dev.virtualearth.net/REST/v1/Locations/US/{item}?o=xml&key=Aowl3fmRvC-iCiFMh_IIb3kG7UAHkcLy8maARHJQS56S4owWmfwxPkJTn8Nt4sTq");
                    HttpResponseMessage response = client.GetAsync($"http://dev.virtualearth.net/REST/v1/Locations/US/{item}?o=xml&key=Aowl3fmRvC-iCiFMh_IIb3kG7UAHkcLy8maARHJQS56S4owWmfwxPkJTn8Nt4sTq").Result; // Replace with your actual API endpoint

                    if (response.IsSuccessStatusCode)
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(response.Content.ReadAsStreamAsync().Result);

                        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                        namespaceManager.AddNamespace("ns", "http://schemas.microsoft.com/search/local/ws/rest/v1");

                        XmlNode xmlNode = xmlDocument.SelectSingleNode("//ns:Response//ns:ResourceSets//ns:ResourceSet//ns:Resources//ns:Location//ns:Point", namespaceManager);

                        if (xmlNode != null)
                        {

                            PointsInformation maps = new PointsInformation
                            {
                                OfficeName = item,
                                Latitude = double.Parse(xmlNode.SelectSingleNode("ns:Latitude", namespaceManager).InnerText, CultureInfo.InvariantCulture),
                                Longitude = double.Parse(xmlNode.SelectSingleNode("ns:Longitude", namespaceManager).InnerText, CultureInfo.InvariantCulture)
                            };

                            listofMaps.Add(maps);
                        }

                    }

                    ;
                }
            }
            return listofMaps;
        }
        private void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the initial map view to North America
            LocationRect northAmericaRect = new LocationRect(new Location(70, -170), new Location(10, -50));
            myMap.SetView(northAmericaRect);

            // Add any other initialization code you may need
        }
    }
}
