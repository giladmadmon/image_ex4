using ImageService.Communication.Singleton;
using ImageService.GUI.Model;
using ImageService.GUI.ViewModel;
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

namespace ImageService.GUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets a value indicating whether the client is connected to the server.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the client is connected; otherwise, <c>false</c>.
        /// </value>
        public bool ClientConnected {
            get {
                return ClientCommunication.Instance.Connected;
            }
        }
    }
}
