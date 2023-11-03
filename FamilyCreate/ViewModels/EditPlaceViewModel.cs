using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class EditPlaceViewModel : NotifyPropertyChangedBase
    {
        private string okbutTxt;
        public string OKButtonText
        {
            get => okbutTxt;
            set
            {
                okbutTxt = value;
                OnPropertyChanged(nameof(OKButtonText));
            }
        }
        public Place Place { get; private set; }
        public EditPlaceView? View { get; private set; }

        private string titletext;

        public string TitleText
        {
            get => titletext;
            set
            {
                titletext = value;
                OnPropertyChanged(nameof(TitleText));
            }
        }

        #region Constructors
        public EditPlaceViewModel()
        {
            Place = new Place();
            OKButtonText = "Добавить";
            TitleText = "Добавление места";
        }

        public EditPlaceViewModel(EditPlaceView view) : this()
        {
            View = view;
        }

        public EditPlaceViewModel(EditPlaceView view, Place place)
        {
            View = view;
            Place = place;
            OKButtonText = "Сохранить";
            TitleText = "Редактирование места";
        }
        #endregion

        public void OK(object obj)
        {
            if (!Place.IsValid)
            {
                MessageBox.Show("Введите всю информацию о месте!", TitleText, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Place.ID != -1)
            {
                App.DatabaseContext!.PlaceTable.Update(Place);
            }
            else
            {
                App.DatabaseContext!.PlaceTable.Add(Place);
                MainWindowViewModel.AddEditPlace = Place;
            }
            View.DialogResult = true;
        }

        public void Cancel(object obj)
        {
            View.DialogResult = false;
        }

        public ICommand OKCommand => new RelayCommand(OK);
        public ICommand CancelCommand => new RelayCommand(Cancel);
    }
}
