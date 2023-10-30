using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class EditPersonViewModel : NotifyPropertyChangedBase
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
        public Person Person
        {
            get => pers;
            set
            {
                pers = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        public List<Place> Places
        {
            get => places;
            set
            {
                places = value;
                OnPropertyChanged(nameof(Places));
            }
        }

        private List<Place> places;
        private Window? parentWindow;
        private Person pers;

        #region Constructors
        public EditPersonViewModel()
        {
            Person = new Person();
            OKButtonText = "Добавить";
            Places = App.DatabaseContext.PlaceTable.ToList();
        }

        public EditPersonViewModel(Window view) : this()
        {
            parentWindow = view;
        }

        public EditPersonViewModel(Window view, Person person)
        {
            parentWindow = view;
            Person = person;
            OKButtonText = "Сохранить";
        }
        #endregion

        private void OK(object obj)
        {
            MainWindowViewModel.AddEditPerson = Person;
            if (Person.ID != -1)
            {
                App.DatabaseContext!.PersonsTable.Update(Person);
            }
            else
            {
                App.DatabaseContext!.PersonsTable.Add(Person);
            }
            parentWindow!.DialogResult = true;
        }

        private void Cancel(object obj)
        {
            parentWindow!.DialogResult = false;
        }

        private void AddBornPlace(object obj)
        {
            EditPlaceView placeView = new EditPlaceView();
            if (placeView.ShowDialog() == true)
            {
                Places = App.DatabaseContext.PlaceTable.ToList();
                Person.BornPlace = App.DatabaseContext.PlaceTable.ElementAt(App.DatabaseContext.PlaceTable.GetIDByItem(MainWindowViewModel.AddEditPlace!));
            }
        }

        private void AddDeathPlace(object obj)
        {
            EditPlaceView placeView = new EditPlaceView();
            if (placeView.ShowDialog() == true)
            {
                Places = App.DatabaseContext.PlaceTable.ToList();
                Person.DeathPlace = App.DatabaseContext.PlaceTable.ElementAt(App.DatabaseContext.PlaceTable.GetIDByItem(MainWindowViewModel.AddEditPlace!));
            }
        }

        public ICommand AddBornCommand => new RelayCommand(AddBornPlace);
        public ICommand AddDeathCommand => new RelayCommand(AddDeathPlace);
        public ICommand OKCommand => new RelayCommand(OK);
        public ICommand CancelCommand => new RelayCommand(Cancel);
    }
}