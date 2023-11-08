using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class AboutProgrammViewModel : NotifyPropertyChangedBase
    {
        public ICommand CloseCommand => new RelayCommand(Close);

        private readonly AboutProgrammView parent;

        public AboutProgrammViewModel() { }
        public AboutProgrammViewModel(AboutProgrammView view)
        {
            parent = view;
        }

        private void Close(object obj) => parent.Close();
    }
}
