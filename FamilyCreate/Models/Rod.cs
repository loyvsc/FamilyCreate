using FamilyCreate.Database;

namespace FamilyCreate.Models
{
    public class Rod : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public int TreeID { get; set; }
        public string Name { get; set; }

        public bool IsValid => Name != string.Empty;
        public string Print => $"Род\nНазвание: {Name}";

        public Rod()
        {
            ID = -1;
            TreeID = -1;
            Name = string.Empty;
        }

        public Rod(int iD, int treeID, string name)
        {
            ID = iD;
            TreeID = treeID;
            Name = name;
        }


        public static bool operator ==(Rod? a, Rod? b)
        {
            bool stat = false;
            if (a?.ID == b?.ID && a?.Name == b?.Name && a?.TreeID == b?.TreeID)
            {
                stat = true;
            }
            return stat;
        }


        public static bool operator !=(Rod? a, Rod? b)
        {
            return !(a == b);
        }
    }
}
