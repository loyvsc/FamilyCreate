using System;
using System.Windows;

namespace FamilyCreate
{
    public partial class App : Application
    {
        public static readonly Database.DatabaseContext DatabaseContext = new Database.DatabaseContext();

    }
}
