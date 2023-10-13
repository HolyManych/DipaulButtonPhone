using System.Windows;

namespace ButtonPhoneEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KeyboardViewModel KeyboardViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            KeyboardViewModel = new KeyboardViewModel();
        }
    }
}
