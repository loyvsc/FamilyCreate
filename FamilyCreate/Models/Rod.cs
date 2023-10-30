﻿using FamilyCreate.Database;

namespace FamilyCreate.Models
{
    public class Rod : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public int TreeID { get; set; }
        public string Name { get; set; }

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
    }
}
