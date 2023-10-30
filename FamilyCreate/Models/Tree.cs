using FamilyCreate.Database;

namespace FamilyCreate.Models
{
    public class Tree : NotifyPropertyChangedBase, ITable
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

        private string name = string.Empty;

        public Tree()
        {
            ID = 0;
            Name = string.Empty;
        }

        public Tree(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public bool IsValid => ID != 0 && Name != string.Empty;
    }
}