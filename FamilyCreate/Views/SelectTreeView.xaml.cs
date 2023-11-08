using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectTreeView.xaml
    /// </summary>
    public partial class SelectTreeView : Window
    {
        public SelectTreeView()
        {
            InitializeComponent();
            DataContext = new SelectTreeViewModel(this);
        }
    }
}
