using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class CreateTreeView : Window
    {
        public CreateTreeView(bool forOpen = false)
        {
            InitializeComponent();
            this.DataContext = new CreateTreeViewModel(this, forOpen);
        }
    }
}