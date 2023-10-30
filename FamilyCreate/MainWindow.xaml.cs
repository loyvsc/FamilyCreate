using System.Windows;
using FamilyCreate.ViewModels;

namespace FamilyCreate
{
    public partial class MainWindow : Window
    {
        public static MainWindowViewModel? ViewModel { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            this.DataContext = ViewModel;
        }
    }
}
