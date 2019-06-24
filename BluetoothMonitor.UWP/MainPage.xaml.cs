using BluetoothListener.Lib;
using System.Collections.ObjectModel;
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

        //private List<BeaconDevice> devices;
        public ObservableCollection<BeaconDevice> Devices;
         

        public MainPage()
        {
            this.InitializeComponent();
            var dispatcher = Window.Current.Dispatcher;
            Devices = new ObservableCollection<BeaconDevice>();
            _listener = new BluetoothListenerManager(Devices, dispatcher);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            _listener.Start();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            _listener.Stop();
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = e;
        }
    }
}
