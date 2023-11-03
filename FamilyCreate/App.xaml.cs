using FamilyCreate.Database;
using System.Windows;

namespace FamilyCreate
{
    public partial class App : Application
    {
        public static DatabaseContext DatabaseContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseContext = new DatabaseContext();
            base.OnStartup(e);
        }
    }
}