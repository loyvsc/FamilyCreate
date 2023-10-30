using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class EditNoteView : Window
    {
        public EditNoteView(Tree currentTree)
        {
            InitializeComponent();
            this.DataContext = new EditNoteViewModel(currentTree, this);
        }
        public EditNoteView(Tree currentTree, Note currentNote)
        {
            InitializeComponent();
            this.DataContext = new EditNoteViewModel(currentTree, this, currentNote);
        }
    }
}