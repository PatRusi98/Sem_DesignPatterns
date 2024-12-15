using System.Windows;
using System.Windows.Controls;

namespace Sem_DesignPatterns
{
    /// <summary>
    /// Interaction logic for GeoSystemMenuView.xaml
    /// </summary>
    public partial class GeoSystemMenuView : UserControl
    {
        public GeoSystemMenuView()
        {
            InitializeComponent();
        }

        private void OnInsertParcelClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Insert Parcel clicked!");
        }

        private void OnSearchParcelClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Search Parcel clicked!");
        }

        private void OnFindAllParcelsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Find all Parcels clicked!");
        }

        private void OnInsertPropertyClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Insert Property clicked!");
        }

        private void OnSearchPropertyClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Search Property clicked!");
        }

        private void OnFindAllPropertiesClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Find all Properties clicked!");
        }

        private void OnFindByPointClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Find by Point clicked!");
        }

        private void OnFindAllObjectsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Find all Objects clicked!");
        }

        private void OnGenerateParcelsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generate Parcels clicked!");
        }

        private void OnGeneratePropertiesClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generate Properties clicked!");
        }
        private void OnGenerateRandomObjectsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generate Random Objects clicked!");
        }

        private void OnRandomOperationsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Random Operations clicked!");
        }
    }
}
