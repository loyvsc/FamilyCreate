using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class CreateTreeView : Window
    {
        public CreateTreeView(Models.Tree? createdTree = null)
        {
            InitializeComponent();
            this.DataContext = new CreateTreeViewModel(this, createdTree);
        }
    }
}