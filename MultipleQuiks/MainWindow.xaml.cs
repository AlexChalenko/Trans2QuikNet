using Microsoft.Win32;
using MultipleQuiks.ViewModels;
using System.IO;
using System.Windows;

namespace MultipleQuiks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainViewModel = new MainViewModel();

            _mainViewModel.AddNewQuik = () => AddNewQuikFromDialog();
        }

        private QuikViewModel? AddNewQuikFromDialog()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Quik|info.exe",
                Multiselect = false
            };
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                var newQuik = new QuikViewModel(Path.GetDirectoryName(openFileDialog.FileName) + Path.DirectorySeparatorChar);
                return newQuik;
            }
            return null;
        }
    }
}