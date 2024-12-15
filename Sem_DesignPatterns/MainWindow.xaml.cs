using Sem_DesignPatterns.UI.Models;
using Sem_DesignPatterns.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Sem_DesignPatterns
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddDynamicColumns();
            LoadMenuView("GeoSystem");
        }

        private void AddDynamicColumns()
        {
            var viewModel = new MainViewModel();
            DataContext = viewModel;

            if (viewModel == null || !viewModel.Items.Any()) return;

            var itemType = viewModel.Items.First().GetType();

            foreach (var property in BaseModel.GetPropertiesWithDescription<BaseModel>())
            {
                var column = new DataGridTextColumn
                {
                    Header = BaseModel.GetPropertyDescription(property),
                    Binding = new System.Windows.Data.Binding(property.Name)
                };

                DynamicGrid.Columns.Add(column);
            }
        }

        public void LoadMenuView(string viewName)
        {
            DynamicMenuContent.Content = viewName switch
            {
                "GeoSystem" => new GeoSystemMenuView(),
                _ => null
            };
        }

        private void OnLoadFromFileClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Loading from file...");
        }

        private void OnSaveToFileClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Saving to file...");
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}