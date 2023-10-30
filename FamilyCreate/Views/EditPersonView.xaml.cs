using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class EditPersonView : Window
    {
        public EditPersonView()
        {
            InitializeComponent();
            this.DataContext = new EditPersonViewModel(this);
        }

        public EditPersonView(Person person)
        {
            InitializeComponent();
            this.DataContext = new EditPersonViewModel(this, person);
        }        
    }
}
