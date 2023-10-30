using FamilyCreate.Database;
using System;

namespace FamilyCreate.Models
{
    public class Source : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public int TreeID
        {
            get => treeId;
            set
            {
                treeId = value;
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

        private int treeId;
        private string? text;
        private DateTime addDate;

        public Source()
        {
            ID = -1;
        }

        public Source(int iD, int treeID, string text, DateTime addDate)
        {
            ID = iD;
            TreeID = treeID;
            Text = text;
            AddDate = addDate;
        }
    }
}
