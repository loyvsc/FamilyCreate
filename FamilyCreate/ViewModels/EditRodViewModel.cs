using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class EditRodViewModel : NotifyPropertyChangedBase
    {
        public Rod CurrentRod
        {
            get => curNot;
            set
            {
                curNot = value;
                OnPropertyChanged(nameof(CurrentRod));
            }
        }

        public int CurrentTreeID { get; set; }
        public EditRodView Parent { get; set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ICommand AddNoteCommand => new RelayCommand(AddNote);
        public ICommand CancelCommand => new RelayCommand((object obj) => Parent.DialogResult = true);

        private string title;
        private Rod curNot;

        public EditRodViewModel() { }

        public EditRodViewModel(Tree currentTree, EditRodView parent, Rod? currentNote = null)
        {
            if (currentNote == null)
            {
                Title = "Добавление рода";
                CurrentRod = new Rod();
            }
            else
            {
                CurrentRod = currentNote;
                Title = "Редактирование рода";
            }
            CurrentTreeID = currentTree.ID;
            Parent = parent;
        }

        private void AddNote(object obj)
        {
            if (!CurrentRod.IsValid)
            {
                MessageBox.Show("Введите всю информацию о роду!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentRod.ID == -1)
            {
                CurrentRod.TreeID = CurrentTreeID;
                App.DatabaseContext.RodsTable.Add(CurrentRod);
            }
            else
            {
                App.DatabaseContext.RodsTable.Update(CurrentRod);
            }
            Parent.DialogResult = true;
        }
    }
}
