using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutProgrammView.xaml
    /// </summary>
    public partial class AboutProgrammView : Window
    {
        public AboutProgrammView()
        {
            InitializeComponent();
            this.DataContext = new AboutProgrammViewModel(this);
        }
    }
}
