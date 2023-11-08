using FamilyCreate.Database;
using System;
using System.Collections.Generic;

namespace FamilyCreate.Models
{
    public class Event : NotifyPropertyChangedBase, ITable
    {
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        public int RodID
        {
            get => rodid;
            set
            {
                rodid = value;
                OnPropertyChanged(nameof(RodID));
            }
        }
        public Rod Rod
        {
            get => App.DatabaseContext.RodsTable.ElementAt(RodID);
            set
            {
                if (value != null)
                {
                    RodID = value.ID;
                }
                OnPropertyChanged(nameof(Rod));
            }
        }
        public int PlaceID
        {
            get => placeid;
            set
            {
                placeid = value;
                OnPropertyChanged(nameof(PlaceID));
            }
        }
        public DateTime? StartDate
        {
            get => startdate;
            set
            {
                startdate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime? EndDate
        {
            get => enddate;
            set
            {
                enddate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        public string? Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public Place Place
        {
            get => App.DatabaseContext!.PlaceTable.ElementAt(PlaceID);
            set
            {
                PlaceID = value.ID;
                OnPropertyChanged(nameof(Place));
            }
        }

        public string? StartDateAsString => StartDate?.ToShortDateString();
        public string? EndDateAsString => EndDate?.ToShortDateString();

        public List<Person> EventPerons
        {
            get => evpers;
            set
            {
                evpers = value;
                OnPropertyChanged(nameof(EventPerons));
            }
        }

        #region Private Values
        private List<Person> evpers;
        private DateTime? startdate;
        private DateTime? enddate;
        private string? text;
        private int rodid;
        private int placeid;
        private int id;
        #endregion

        public Event()
        {
            ID = -1;
            RodID = -1;
            PlaceID = -1;
            Text = string.Empty;
            EventPerons = new List<Person>();
        }

        public Event(int id, int rodId, int placeId, DateTime? start, DateTime? end, string text) : this()
        {
            ID = id;
            RodID = rodId;
            PlaceID = placeId;
            StartDate = start;
            EndDate = end;
            Text = text;
            EventPerons = App.DatabaseContext!.PersonsTable.Select($"SELECT * FROM Persons WHERE ID in (SELECT PersonID FROM EventPersons WHERE EventID = {ID});");
        }

        public bool IsValid =>
            RodID != -1 &&
            PlaceID != -1 &&
            StartDate != null &&
            Text != string.Empty;

        public string Print
        {
            get
            {
                string persons = string.Empty;
                foreach(Person item in App.DatabaseContext.PersonsTable.ToList())
                {
                    persons += item.FIO + "\n";
                }
                return $"Событие\nДата начала: {StartDateAsString}\nДата окончания: " +
                    $"{EndDateAsString}\nОписание: {Text}\nУчавствующие лица: \n{persons}";
            }
        }            
    }
}
