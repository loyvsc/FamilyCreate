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
        public List<Rod> Rods
        {
            get => rods;
            set
            {
                rods = value;
                OnPropertyChanged(nameof(Rods));
            }
        }
        public List<Person> MothersList
        {
            get => motlst;
            set
            {
                motlst = value;
                OnPropertyChanged(nameof(MothersList));
            }
        }
        public List<Person> FathersList
        {
            get => fatlst;
            set
            {
                fatlst = value;
                OnPropertyChanged(nameof(FathersList));
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string title;
        private List<Person> fatlst;
        private List<Person> motlst;
        private List<Place> places;
        private List<Rod> rods;
        private readonly Window? parentWindow;
        private Person pers;

        #region Constructors
        public EditPersonViewModel()
        {
            Person = new Person();
            OKButtonText = "Добавить";
            Places = App.DatabaseContext.PlaceTable.ToList();
            Rods = App.DatabaseContext.RodsTable.ToList();
            Title = "Добавление персоны";
            MothersList = App.DatabaseContext.PersonsTable.Select("SELECT * FROM Persons WHERE ismale = 0;");            
        }

        public EditPersonViewModel(Window view) : this()
        {
            parentWindow = view;
        }

        public EditPersonViewModel(Window view, Person person) : this(view)
        {
            Person = person;
            OKButtonText = "Сохранить";
            Title = "Редактирование персоны";
        }
        #endregion

        private void OK(object obj)
        {
            if (!Person.IsValid)
            {
                MessageBox.Show("Введите всю информацию о персоне!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Person.ID != -1)
            {
                App.DatabaseContext.PersonsTable.Update(Person);
            }
            else
            {
                App.DatabaseContext.PersonsTable.Add(Person);
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
            }
        }

        private void AddDeathPlace(object obj)
        {
            EditPlaceView placeView = new EditPlaceView();
            if (placeView.ShowDialog() == true)
            {
                Places = App.DatabaseContext.PlaceTable.ToList();
            }
        }

        public ICommand AddBornCommand => new RelayCommand(AddBornPlace);
        public ICommand AddDeathCommand => new RelayCommand(AddDeathPlace);
        public ICommand OKCommand => new RelayCommand(OK);
        public ICommand CancelCommand => new RelayCommand(Cancel);
    }
}