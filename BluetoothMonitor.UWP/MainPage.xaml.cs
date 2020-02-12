using System;
using BluetoothListener.Lib;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BluetoothMonitor.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly BluetoothListenerManager _listener;
        public ViewData Data;


        public MainPage()
        {
            this.InitializeComponent();
            var dispatcher = Window.Current.Dispatcher;
            Data = new ViewData();
            _listener = new BluetoothListenerManager(Data, dispatcher, new BeaconCache());
            DataContext = Data;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = Data;
            _listener.Start();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {

            _listener.Stop();
            DataContext = null;
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private  async void BtInfo_Click(object sender, RoutedEventArgs e)
        {
            var memoryUsage = MemoryManager.AppMemoryUsage;

            var memoryUsageInMb = (float) memoryUsage / 1024 / 1024;

            var infoText = $"Memory Usage: {memoryUsageInMb:F} Mb {Environment.NewLine}Beacons count: {Data.Devices.Count()}";

            var messageDialog = new MessageDialog(infoText);

           // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}
