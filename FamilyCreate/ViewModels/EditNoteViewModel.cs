using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class EditNoteViewModel : NotifyPropertyChangedBase
    {
        public Note CurrentNote
        {
            get => curNot;
            set
            {
                curNot = value;
                OnPropertyChanged(nameof(CurrentNote));
            }
        }


        public int CurrentTreeID { get; set; }
        public EditNoteView Parent { get; set; }

        public ICommand AddNoteCommand => new RelayCommand(AddNote);
        public ICommand CancelCommand => new RelayCommand((object obj) => Parent.DialogResult = true);

        private Note curNot;

        public EditNoteViewModel() { }

        public EditNoteViewModel(Tree currentTree, EditNoteView parent, Note? currentNote = null)
        {
            if (currentNote == null)
            {
                parent.Title = "Написание заметки";
                CurrentNote = new Note();
            }
            else
            {
                CurrentNote = currentNote;
                parent.Title = "Редактирование заметки";
            }
            CurrentTreeID = currentTree.ID;
            Parent = parent;
        }

        private void AddNote(object obj)
        {
            if (!CurrentNote.IsValid)
            {
                MessageBox.Show("Введите всю информацию о заметке!", "Заметка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentNote.ID == -1)
            {
                CurrentNote.AddDate = System.DateTime.Now;
                CurrentNote.TreeID = CurrentTreeID;
                App.DatabaseContext.NoteTable.Add(CurrentNote);
            }
            else
            {
                App.DatabaseContext.NoteTable.Update(CurrentNote);
            }
            Parent.DialogResult = true;
        }
    }
}
