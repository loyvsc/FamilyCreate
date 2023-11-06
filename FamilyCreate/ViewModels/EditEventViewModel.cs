using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class EditEventViewModel : NotifyPropertyChangedBase
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
        public Event Event
        {
            get => evn;
            set
            {
                evn = value;
                OnPropertyChanged(nameof(Event));
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
        public int CurrentTreeID
        {
            get => tree;
            set
            {
                tree = value;
                OnPropertyChanged(nameof(CurrentTreeID));
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
        public int SelectedPlaceIndex
        {
            get => selplcinx;
            set
            {
                selplcinx = value;
                if (value != -1)
                {
                    int placeid = Places[value].ID;
                    Event.PlaceID = placeid;
                }
                OnPropertyChanged(nameof(SelectedPlaceIndex));
            }
        }
        public int SelectedRodIndex
        {
            get => selrodinx;
            set
            {
                selrodinx = value;
                if (value != -1)
                {
                    int rodid = Rods[value].ID;
                    Event.RodID = rodid;
                    PersonsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM PERSONS WHERE RODID = {rodid};");
                }
                OnPropertyChanged(nameof(SelectedRodIndex));
            }
        }

        private int selplcinx;
        private int selrodinx;

        public Person? SelectedPersonForAdd
        {
            get => selprs;
            set
            {
                selprs = value;
                OnPropertyChanged(nameof(SelectedPersonForAdd));
            }
        }
        public Person? SelectedListPerson
        {
            get => sellistprs;
            set
            {
                sellistprs = value;
                OnPropertyChanged(nameof(SelectedListPerson));
            }
        }
        public List<Person>? PersonsList
        {
            get => prslst;
            set
            {
                prslst = value;
                OnPropertyChanged(nameof(PersonsList));
            }
        }
        public List<Person>? PersonsAddList
        {
            get => prsaddlst;
            set
            {
                prsaddlst = value;
                OnPropertyChanged(nameof(PersonsAddList));
            }
        }

        #region Private Values
        private Person? selprs;
        private Person? sellistprs;
        private List<Person>? prslst;
        private List<Person>? prsaddlst;
        private List<Rod> rods;
        private string? title;
        private List<Place> places;
        private readonly EditEventView parentWindow;
        private Event evn;
        private int tree;
        #endregion

        #region Constructors
        public EditEventViewModel()
        {
            Event = new Event();
            PersonsAddList = new List<Person>();
            Places = App.DatabaseContext.PlaceTable.ToList();
            OKButtonText = "Добавить";
        }

        public EditEventViewModel(EditEventView view, Tree currentTree) : this()
        {
            SelectedRodIndex = -1;
            SelectedPlaceIndex = -1;
            CurrentTreeID = currentTree.ID;
            Rods = App.DatabaseContext.RodsTable.Select($"SELECT * FROM RODS WHERE TREEID={CurrentTreeID};");
            parentWindow = view;
            parentWindow.Title = "Добавление события";
        }

        public EditEventViewModel(EditEventView view, Tree currentTree, Event person)
        {
            Places = App.DatabaseContext.PlaceTable.ToList();
            CurrentTreeID = currentTree.ID;
            Rods = App.DatabaseContext.RodsTable.Select($"SELECT * FROM RODS WHERE TREEID={CurrentTreeID};");
            Event = person;
            parentWindow = view;
            PersonsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM PERSONS WHERE RODID={Event.RodID};");
            if (Event.EventPerons.Count > 0)
            {
                foreach (var item in Event.EventPerons)
                {
                    int index = PersonsList.IndexOf(item);
                    if (index!=-1) PersonsList.RemoveAt(index);
                }
                parentWindow.persList.Items.Refresh();
            }
            parentWindow.Title = "Редактирование события";
            OKButtonText = "Сохранить";
            PersonsAddList = person.EventPerons;

            SelectedRodIndex = Rods.FindIndex(x => x.ID == Event.RodID);
            SelectedPlaceIndex = Places.FindIndex(x => x.ID == Event.PlaceID);
        }
        #endregion

        private void OK(object obj)
        {
            Event.Place = Places[SelectedPlaceIndex];
            Event.Rod = Rods[SelectedRodIndex];
            if (Event.ID != -1)
            {
                if (Event.IsValid)
                {
                    UpdateEventPersons();
                    App.DatabaseContext.EventTable.Update(Event);
                }
                else
                {
                    MessageBox.Show("Введите всю информацию!", "Добавление события", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                App.DatabaseContext.EventTable.Add(Event);
                Event = App.DatabaseContext.EventTable.Select("SELECT * FROM EVENTS WHERE ID = (SELECT MAX(ID) FROM EVENTS);")[0];
                Event.EventPerons = PersonsAddList;
                UpdateEventPersons();
            }
            parentWindow!.DialogResult = true;
        }

        private void UpdateEventPersons()
        {
            App.DatabaseContext.Query($"DELETE FROM EVENTPERSONS WHERE EVENTID = {Event.ID};");
            foreach (var item in Event.EventPerons)
            {
                App.DatabaseContext.EventActorsTable.Add(new EventPerson(-1, item.ID, Event.ID));
            }
        }

        private void Cancel(object obj)
        {
            parentWindow!.DialogResult = false;
        }

        private void AddPlace(object obj)
        {
            EditPlaceView placeView = new EditPlaceView();
            if (placeView.ShowDialog() == true)
            {
                Places = App.DatabaseContext.PlaceTable.ToList();
            }
        }

        private void AddEventPerson(object obj)
        {
            if (SelectedListPerson == null) return;
            PersonsAddList.Add(SelectedListPerson);
            PersonsList?.Remove(SelectedListPerson);
            parentWindow.persList.Items.Refresh();
            parentWindow.persaddList.Items.Refresh();

            OnPropertyChanged(nameof(PersonsAddList));
            OnPropertyChanged(nameof(PersonsList));
        }

        private void DeleteEventPerson(object obj)
        {
            if (SelectedPersonForAdd == null) return;
            PersonsAddList.Remove(SelectedPersonForAdd);
            PersonsList?.Add(SelectedPersonForAdd);
            parentWindow.persList.Items.Refresh();
            parentWindow.persaddList.Items.Refresh();

            OnPropertyChanged(nameof(PersonsAddList));
            OnPropertyChanged(nameof(PersonsList));
        }

        public ICommand AddPlaceCommand => new RelayCommand(AddPlace);
        public ICommand OKCommand => new RelayCommand(OK);
        public ICommand AddEventPersonCommand => new RelayCommand(AddEventPerson);
        public ICommand DeleteEventPersonCommand => new RelayCommand(DeleteEventPerson);
        public ICommand CancelCommand => new RelayCommand(Cancel);
    }
}