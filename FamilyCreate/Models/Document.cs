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
        public string? Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public string? FileStream
        {
            get => filestream;
            set
            {
                filestream = value;
                OnPropertyChanged(nameof(FileStream));
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

        private int treeid;
        private int surcid;        
        private string text;
        private string filestream;
        private Source source;
        private DateTime addDate;

        public Document()
        {
            ID = -1;
            TreeID = -1;
            SourceID = -1;
        }

        public Document(int iD, int treeID, string text, string fileStream, DateTime addDate, int sourceid)
        {
            ID = iD;
            TreeID = treeID;
            Text = text;
            FileStream = fileStream;
            AddDate = addDate;
            SourceID = sourceid;
        }
    }
}
