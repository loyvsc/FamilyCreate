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
        public int PlaceID
        {
            get => placeid;
            set
            {
                placeid = value;
                OnPropertyChanged(nameof(PlaceID));
            }
        }
        public DateTime StartDate
        {
            get => startdate;
            set
            {
                startdate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime EndDate
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

        public Place Place => App.DatabaseContext!.PlaceTable.ElementAt(PlaceID);
        public List<Person> EventPerons => App.DatabaseContext!.PersonsTable.Select($"SELECT * FROM Persons WHERE ID in (SELECT PersonID FROM EventPersons WHERE EventID = {ID});");

        private DateTime startdate;
        private DateTime enddate;
        private string? text;
        private int rodid;
        private int placeid;
        private int id;

        public Event()
        {
            ID = -1;
        }

        public Event(int id, int rodId, int placeId, DateTime start, DateTime end, string text)
        {
            ID = id;
            RodID = rodId;
            PlaceID = placeId;
            StartDate = start;
            EndDate = end;
            Text = text;
        }
    }
}
