using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class EditEventView : Window
    {
        public EditEventView()
        {
            InitializeComponent();
        }
        public EditEventView(Tree currentTree) : this()
        {
            this.DataContext = new EditEventViewModel(this, currentTree);
        }
        public EditEventView(Tree currentTree, Event currentNote)
        {
            InitializeComponent();
            this.DataContext = new EditEventViewModel(this, currentTree, currentNote);
        }
    }
}