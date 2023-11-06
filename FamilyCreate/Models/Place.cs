using FamilyCreate.Database;

namespace FamilyCreate.Models
{
    public class Place : NotifyPropertyChangedBase, ITable
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
        public string? Latitude
        {
            get => lat;
            set
            {
                lat = value;
                OnPropertyChanged(nameof(Latitude));
            }
        }
        public string? Longitude
        {
            get => lon;
            set
            {
                lon = value;
                OnPropertyChanged(nameof(Longitude));
            }
        }

        public string? Description
        {
            get => desc;
            set
            {
                desc = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int id;
        private string? lat;
        private string? lon;
        private string? desc;
        private string? name;

        public bool IsValid => Name != string.Empty && Description != string.Empty
            && Longitude != string.Empty && Latitude != string.Empty;

        public Place()
        {
            ID = -1;
            Name = string.Empty;
        }

        public Place(int iD, string lon, string lat, string name, string desc) : this()
        {
            ID = iD;
            Latitude = lat;
            Longitude = lon;
            Name = name;
            Description = desc;
        }

        public override string ToString()
        {
            if (this == null)
            {
                return "-";
            }
            else
            {
                return Name!;
            }
        }
    }
}