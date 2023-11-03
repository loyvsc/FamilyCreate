using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
