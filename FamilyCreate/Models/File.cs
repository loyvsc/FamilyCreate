using FamilyCreate.Database;

namespace FamilyCreate.Models
{
    public class File : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public byte[] FileText
        {
            get => file;
            set
            {
                file = value;
                OnPropertyChanged(nameof(File));
            }
        }
        public string Extension
        {
            get => ext;
            set
            {
                ext = value;
                OnPropertyChanged(nameof(Extension));
            }
        }

        #region Private Vars
        private string name;
        private byte[] file;
        private string ext;
        #endregion

        public File()
        {
            ID = -1;
            Name = string.Empty;
            FileText = new byte[0];
            Extension = string.Empty;
        }

        public File(int id, string name, byte[] text, string extension)
        {
            ID = id;
            Name = name;
            FileText = text;
            Extension = extension;
        }

        public bool IsValid =>
            Name != string.Empty &&
            FileText !=  new byte[0] &&
            Extension != string.Empty;
    }
}