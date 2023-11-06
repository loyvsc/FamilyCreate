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

        public ICommand AddNoteCommand => new RelayCommand(AddNote);
        public ICommand CancelCommand => new RelayCommand((object obj) => Parent.DialogResult = true);

        private Rod curNot;

        public EditRodViewModel() { }

        public EditRodViewModel(Tree currentTree, EditRodView parent, Rod? currentNote = null)
        {
            if (currentNote == null)
            {
                parent.Title = "Добавление рода";
                CurrentRod = new Rod();
            }
            else
            {
                CurrentRod = currentNote;
                parent.Title = "Редактирование рода";
            }
            CurrentTreeID = currentTree.ID;
            Parent = parent;
        }

        private void AddNote(object obj)
        {
            if (!CurrentRod.IsValid)
            {
                MessageBox.Show("Введите всю информацию о роду!", Parent.Title, MessageBoxButton.OK, MessageBoxImage.Error);
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
