using FamilyCreate.Database;
using System;

namespace FamilyCreate.Models
{
    public class Note : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public int TreeID
        {
            get => treeid;
            set
            {
                treeid = value;
                OnPropertyChanged(nameof(TreeID));
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
        public DateTime AddDate
        {
            get => addDate;
            set
            {
                addDate = value;
                OnPropertyChanged(nameof(AddDate));
            }
        }

        private int treeid;
        private string? text;
        private DateTime addDate;

        public Note()
        {
            ID = -1;
            TreeID = -1;
        }

        public Note(int iD, int treeID, string text, DateTime date)
        {
            ID = iD;
            TreeID = treeID;
            Text = text;
            AddDate = date;
        }
    }
}
