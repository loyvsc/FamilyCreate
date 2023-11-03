using FamilyCreate.Database;
using System;

namespace FamilyCreate.Models
{
    public class Document : NotifyPropertyChangedBase, ITable
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
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
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
        public string AddDateAsString => AddDate.Date.ToShortDateString();
        public Source? Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }
        public int SourceID
        {
            get => surcid;
            set
            {
                surcid = value;
                OnPropertyChanged(nameof(SourceID));
            }
        }
        public int FileID
        {
            get => fileId;
            set
            {
                fileId = value;
                OnPropertyChanged(nameof(FileID));
            }
        }
        public File File
        {
            get => App.DatabaseContext.FileTable.ElementAt(FileID);
            set
            {
                if (File != null)
                {
                    FileID = value.ID;
                    App.DatabaseContext.FileTable.Update(value);
                }
            }
        }

        public bool IsValid => Text != string.Empty && Name != string.Empty;

        #region Private Vars
        private int fileId;
        private int treeid;
        private int surcid;
        private string? text;
        private string? name;
        private Source? source;
        private DateTime addDate;
        #endregion

        public Document()
        {
            ID = -1;
            TreeID = -1;
            FileID = -1;
            SourceID = -1;
            Name = string.Empty;
            Text = string.Empty;
        }

        public Document(int iD, int treeID, string text, int fileid, DateTime addDate, int sourceid,string name)
        {
            ID = iD;
            TreeID = treeID;
            Text = text;
            AddDate = addDate;
            SourceID = sourceid;
            FileID = fileid;
            Name = name;
        }
    }
}