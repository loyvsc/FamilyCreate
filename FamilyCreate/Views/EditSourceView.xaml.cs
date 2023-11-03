using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace FamilyCreate.Views
{
    public partial class EditSourceView : Window
    {
        public EditSourceView(Tree currentTree)
        {
            InitializeComponent();
            this.DataContext = new EditSourceViewModel(this,currentTree);
        }
        public EditSourceView(Tree currentTree, Source currentSource)
        {
            InitializeComponent();
            this.DataContext = new EditSourceViewModel(this, currentTree, currentSource);
        }
    }
}