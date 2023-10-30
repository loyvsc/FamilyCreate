using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class EditPlaceView : Window
    {
        private EditPlaceViewModel viewModel;
        public EditPlaceView()
        {
            InitializeComponent();
            viewModel = new EditPlaceViewModel(this);
            this.DataContext = viewModel;
        }

        public EditPlaceView(Place place)
        {
            InitializeComponent();
            viewModel = new EditPlaceViewModel(this, place);
            this.DataContext = viewModel;
        }
    }
}
