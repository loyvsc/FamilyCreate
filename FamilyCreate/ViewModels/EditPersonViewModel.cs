using FamilyCreate.Models;
using FamilyCreate.Views;
using System;
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

        #region Lists
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
        #endregion

        public int SelectedFatherIndex
        {
            get => selfatind;
            set
            {
                selfatind = value;
                OnPropertyChanged(nameof(SelectedFatherIndex));
            }
        }
        public int SelectedMotherIndex
        {
            get => selmatind;
            set
            {
                selmatind = value;
                OnPropertyChanged(nameof(SelectedMotherIndex));
            }
        }
        public int SelectedRodIndex
        {
            get => selrodind;
            set
            {
                selrodind = value;
                OnPropertyChanged(nameof(SelectedRodIndex));
            }
        }
        public int SelectedBornPlaceIndex
        {
            get => selbornplcind;
            set
            {
                selbornplcind = value;
                OnPropertyChanged(nameof(SelectedBornPlaceIndex));
            }
        }
        public int SelectedDeathPlaceIndex
        {
            get => seldethplcind;
            set
            {
                seldethplcind = value;
                OnPropertyChanged(nameof(SelectedDeathPlaceIndex));
            }
        }

        #region Private vars
        private int selrodind;
        private int selbornplcind;
        private int seldethplcind;
        private int selfatind;
        private int selmatind;
        private List<Person> fatlst;
        private List<Person> motlst;
        private List<Place> places;
        private List<Rod> rods;
        private readonly Window? parentWindow;
        private Person pers;
        #endregion

        #region Constructors
        public EditPersonViewModel()
        {
            SelectedRodIndex = -1;
            SelectedMotherIndex = -1;
            SelectedFatherIndex = -1;
            SelectedBornPlaceIndex = -1;
            SelectedDeathPlaceIndex = -1;
            Person = new Person();
            Person.IsMale = true;
            OKButtonText = "Добавить";
            MothersList = App.DatabaseContext.PersonsTable.Select("SELECT * FROM mothers");
            FathersList = App.DatabaseContext.PersonsTable.Select("SELECT * FROM fathers");
            Places = App.DatabaseContext.PlaceTable.ToList();
            Rods = App.DatabaseContext.RodsTable.ToList();
        }

        public EditPersonViewModel(Window view) : this()
        {
            parentWindow = view;
            parentWindow.Title = "Добавление персоны";
        }

        public EditPersonViewModel(Window view, Person person)
        {
            MothersList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM mothers WHERE ID !={person.ID};");
            FathersList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM fathers WHERE ID !={person.ID};");
            Places = App.DatabaseContext.PlaceTable.ToList();
            Rods = App.DatabaseContext.RodsTable.ToList();
            parentWindow = view;
            Person = person;
            OKButtonText = "Сохранить";
            parentWindow.Title = "Редактирование персоны";
            SelectedBornPlaceIndex = Places.IndexOf(Places.Find((x) => x.ID == person.BornPlaceID));
            SelectedDeathPlaceIndex = Places.IndexOf(Places.Find((x) => x.ID == person.DeathPlaceID));
            SelectedRodIndex = Rods.IndexOf(Rods.Find((x) => x.ID == person.RodID));            
            if (person.FatherID != 0)
            {
                SelectedFatherIndex = FathersList.IndexOf(FathersList.Find((x) => x.ID == person.FatherID));
            }
            else
            {
                SelectedFatherIndex = -1;
            }
            if (person.MotherID != 0)
            {
                SelectedMotherIndex = MothersList.IndexOf(MothersList.Find((x) => x.ID == person.MotherID));
            }
            else
            {
                SelectedMotherIndex = -1;
            }
        }
        #endregion

        private void OK(object obj)
        {
            if (SelectedMotherIndex != -1)
            {
                Person.MotherID = MothersList[SelectedMotherIndex].ID;
            }
            if (SelectedFatherIndex != -1)
            {
                Person.FatherID = FathersList[SelectedFatherIndex].ID;
            }
            if (SelectedRodIndex != -1)
            {
                Person.RodID = Rods[SelectedRodIndex].ID;
            }
            if (SelectedBornPlaceIndex != -1)
            {
                Person.BornPlaceID = Places[SelectedBornPlaceIndex].ID;
            }
            if (SelectedDeathPlaceIndex != -1)
            {
                Person.DeathPlaceID = Places[SelectedDeathPlaceIndex].ID;
            }
            if (!Person.IsValid)
            {
                MessageBox.Show("Введите всю информацию о персоне!", parentWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Cancel(object obj) => parentWindow!.DialogResult = false;

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