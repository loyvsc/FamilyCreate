﻿using FamilyCreate.Database;
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
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string AddDateAsString => AddDate.Date.ToShortDateString();

        #region Private Vars
        private string name;
        private int treeId;
        private string? text;
        private DateTime addDate;
        #endregion

        public Source()
        {
            ID = -1;
            AddDate = DateTime.Now;
            Name = string.Empty;
        }

        public static bool operator == (Source left, Source right)
        {
            return left.ID == right.ID && left.AddDate == right.AddDate
                && left.Name == right.Name && left.Text == right.Text;
        }

        public static bool operator !=(Source left, Source right)
        {
            return left.ID != right.ID || left.AddDate != right.AddDate
                || left.Name != right.Name || left.Text != right.Text;
        }

        public Source(int iD, int treeID, string text, DateTime addDate, string name)
        {
            ID = iD;
            TreeID = treeID;
            Text = text;
            AddDate = addDate;
            Name = name;
        }

        public bool IsValid =>
            Name != string.Empty;
    }
}
