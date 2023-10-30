using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class EditRodView : Window
    {
        public EditRodView(Tree currentTree)
        {
            InitializeComponent();
            this.DataContext = new EditRodViewModel(currentTree, this);
        }
        public EditRodView(Tree currentTree, Rod currentRod)
        {
            InitializeComponent();
            this.DataContext = new EditRodViewModel(currentTree, this, currentRod);
        }
    }
}