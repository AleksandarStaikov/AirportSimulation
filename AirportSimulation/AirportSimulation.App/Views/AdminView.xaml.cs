using System.Windows;
using System.Windows.Controls;
using AirportSimulation.Common.ViewModels;

namespace AirportSimulation.App.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AirportSimulation.Common.ViewModels.AdminViewModel adminViewModelObject =
                new AirportSimulation.Common.ViewModels.AdminViewModel();
            adminViewModelObject.InjectVariables();

            DataContext = adminViewModelObject;

            int CheckInStationsCount = int.Parse(checkInStationsCount.Text);

            int PscConveyorsCount = int.Parse(pscConveyorsCount.Text);

            int PscInvalidationPercentage = int.Parse(pscInvalidationPercentage.Text);

            int AscStaffCount = int.Parse(ascStaffCount.Text);

            int BsuCapacity = int.Parse(bsuCapacity.Text);

            int BsuRobotsCount = int.Parse(bsuRobotsCount.Text);

            int AaCount = int.Parse(aaCount.Text);

            int DistanceFromMpaToAa = int.Parse(distanceFromAscToMpu.Text);

            int DistanceFromMpaToPickUp = int.Parse(distanceFromMpaToPickUp.Text);

            int DistanceFromCheckInToPsc = int.Parse(distanceFromCheckInToPsc.Text);

            int DistanceFromPscToMpu = int.Parse(distanceFromPscToMpu.Text);

            int DistanceFromAscToMpu = int.Parse(distanceFromAscToMpu.Text);

            int DistanceFromPscToAsc = int.Parse(distanceFromPscToAsc.Text);

            int PickUpRate = int.Parse(pickUpRate.Text);

            int DropOffRate = int.Parse(dropOffRate.Text);


        }
    }
}
