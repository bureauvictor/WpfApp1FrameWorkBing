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

            AddPoint(new Location(47.6062, -122.3321), "Nombre Oficina 1");
            AddPoint(new Location(37.7749, -122.4194), "Nombre Oficina 1");
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
            for (int i = 0; myMap.Children.Count <= i; i++)
            {
                PointsInformation point = new PointsInformation();
                Location samplePoint = myMap.Children[i].ReadLocalValue(MapLayer.PositionProperty) as Location;
                point.DistanceKM = CalculateDistance(clickedLocation, samplePoint);

                // Assuming i is the index of the Pushpin in myMap.Children
                Pushpin pushpin = myMap.Children[i] as Pushpin;

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
            

            MessageBox.Show($"Distance from closest office: {listofpoints.OrderBy(num => Math.Abs(num.DistanceKM)).Select(num => num.DistanceKM).First()} kilometers; {listofpoints.OrderBy(num => Math.Abs(num.DistanceKM)).Select(num => num.OfficeName).First()}");
            MessageBox.Show($"Distance from farest office: {listofpoints.OrderByDescending(num => Math.Abs(num.DistanceKM)).Select(num => num.DistanceKM).First()} kilometers; {listofpoints.OrderByDescending(num => Math.Abs(num.DistanceKM)).Select(num => num.OfficeName).First()}");
        }
    }
}
