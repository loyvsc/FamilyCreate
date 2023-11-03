using FamilyCreate.Database;
using System;

namespace FamilyCreate.Models
{
    public class Person : NotifyPropertyChangedBase, ITable
    {
        public string? FIO => Surname + " " + Name + " " + Patronomyc;
        public int? Age => DateTime.Now.Year - BornDate?.Year;
        public bool IsAlive => DeathDate == null;
        public string Sex => IsMale ? "М" : "Ж";

        public int ID { get; set; }
        public int RodID { get; set; }

        public Rod? Rod
        {
            get => rod;
            set
            {
                rod = value;
                if (value != null)
                {
                    RodID = value.ID;
                }
                OnPropertyChanged(nameof(Rod));
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
        public string? Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public string? Patronomyc
        {
            get => patronomyc;
            set
            {
                patronomyc = value;
                OnPropertyChanged(nameof(Patronomyc));
            }
        }

        public DateTime? BornDate
        {
            get => bornDate;
            set
            {
                bornDate = value;
                OnPropertyChanged(nameof(BornDate));
            }
        }

        public Place? BornPlace
        {
            get => bornplace;
            set
            {
                if (value != null)
                {
                    bornplace = value;
                    BornPlaceID = value!.ID;
                }
                OnPropertyChanged(nameof(BornPlace));
            }
        }
        public int BornPlaceID
        {
            get => bornPlaceid;
            set
            {
                bornPlaceid = value;
                OnPropertyChanged(nameof(BornPlace));
                OnPropertyChanged(nameof(BornPlaceID));
            }
        }

        public DateTime? DeathDate
        {
            get => deathDate;
            set
            {
                deathDate = value;
                OnPropertyChanged(nameof(DeathDate));
            }
        }
        public Place? DeathPlace
        {
            get => deathplace;
            set
            {
                if (value != null)
                {
                    deathPlaceid = value.ID;
                    deathplace = value;
                }
                OnPropertyChanged(nameof(DeathPlace));
            }
        }

        public int? DeathPlaceID
        {
            get => deathPlaceid;
            set
            {
                deathPlaceid = value;
                OnPropertyChanged(nameof(DeathPlaceID));
                OnPropertyChanged(nameof(DeathPlace));
            }
        }
        public bool IsMale
        {
            get => isMale;
            set
            {
                isMale = value;
                OnPropertyChanged(nameof(isMale));
            }
        }
        public int FatherID
        {
            get => fatherId;
            set
            {
                fatherId = value;
                OnPropertyChanged(nameof(FatherID));
            }
        }
        public int MotherID
        {
            get => motherId;
            set
            {
                motherId = value;
                OnPropertyChanged(nameof(MotherID));
            }
        }
        public Person? Mother
        {
            get => mother;
            set
            {
                mother = value;
                MotherID = value!.ID;
                OnPropertyChanged(nameof(Mother));
            }
        }
        public Person? Father
        {
            get => father;
            set
            {
                father = value;
                FatherID = value!.ID;
                OnPropertyChanged(nameof(Father));
            }
        }

        private string? name;
        private string? surname;
        private string? patronomyc;
        private DateTime? bornDate;
        private Place? bornplace;
        private Place? deathplace;
        private int bornPlaceid;
        private DateTime? deathDate;
        private int? deathPlaceid;
        private bool isMale;
        private int fatherId;
        private int motherId;
        private Person? father;
        private Person? mother;
        private Rod? rod;

        public Person()
        {
            ID = -1;
            Name = string.Empty;
            Surname = string.Empty;
            Patronomyc = string.Empty;
            RodID = -1;
            FatherID = -1;
            MotherID = -1;
            BornPlaceID = -1;
        }

        public Person(int iD, int rodID, string name, string surname, string patronomyc,
            DateTime borndate, int bornplaceid, DateTime? deathdate, int? deathplaceid, bool isMale, int fatherid,
            int motherid)
        {
            ID = iD;
            RodID = rodID;
            Name = name;
            Surname = surname;
            Patronomyc = patronomyc;
            BornDate = borndate;
            BornPlaceID = bornplaceid;
            DeathDate = deathdate;
            DeathPlaceID = deathplaceid;
            IsMale = isMale;
            FatherID = fatherid;
            MotherID = motherid;
            BornPlace = App.DatabaseContext.PlaceTable.ElementAt(BornPlaceID);
            if (DeathPlaceID != null)
            {
                DeathPlace = App.DatabaseContext.PlaceTable.ElementAt((int)DeathPlaceID);
            }
        }

        public override string ToString() => FIO!;

        public bool IsValid =>
            RodID != -1 &&
            BornPlaceID != -1 &&
            (Name != string.Empty || Surname != string.Empty || Patronomyc!=string.Empty) &&
            Patronomyc != string.Empty &&
            BornDate != null;

    }
}