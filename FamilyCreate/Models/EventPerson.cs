using FamilyCreate.Database;

namespace FamilyCreate.Models
{
    public class EventPerson : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int EventID { get; set; }

        public EventPerson()
        {
            ID = -1;
            PersonID = -1;
            EventID = -1;
        }

        public EventPerson(int id, int treeID, int eventID)
        {
            ID = id;
            PersonID = treeID;
            EventID = eventID;
        }
    }
}
