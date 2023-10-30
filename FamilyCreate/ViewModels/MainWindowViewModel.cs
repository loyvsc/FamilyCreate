using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        public Tree? CurrentTree
        {
            get => currentTree;
            set
            {
                currentTree = value;
                OnPropertyChanged(nameof(CurrentTree));
            }
        }

        public Person? SelectedPerson
        {
            get => currentPerson;
            set
            {
                currentPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public Note? SelectedNote
        {
            get => selnot;
            set
            {
                selnot = value;
                OnPropertyChanged(nameof(SelectedNote));
            }
        }

        public Rod? SelectedRod
        {
            get => selrod;
            set
            {
                selrod = value;
                OnPropertyChanged(nameof(SelectedRod));
            }
        }

        private Rod? selrod;
        private Note? selnot;

        public bool IsTreeCreatedOrOpen
        {
            get => isTreeCrOrOp;
            set
            {
                isTreeCrOrOp = value;
                OnPropertyChanged(nameof(IsTreeCreatedOrOpen));
            }
        }

        public MainWindowViewModel()
        {
            IsTreeCreatedOrOpen = false;
        }

        public TabItem? SelectedTabItem
        {
            get => seltabitem;
            set
            {
                seltabitem = value;
                OnPropertyChanged(nameof(SelectedTabItem));
            }
        }
        public object SelectedTab
        {
            get => seltab;
            set
            {
                seltab = value;
                SelectedTabItem = value as TabItem;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        private object seltab;
        private TabItem? seltabitem;
        private Place selplc;

        public Place SelectedPlace
        {
            get => selplc;
            set
            {
                selplc = value;
                OnPropertyChanged(nameof(SelectedPlace));
            }
        }

        public static Person? AddEditPerson { get; set; }
        public static Place? AddEditPlace { get; set; }
        public bool IsSelectedPersonNotNull => SelectedPerson != null;
        public bool IsCurrentTreeSelected => CurrentTree != null;
        public bool IsPrintEnabled => App.DatabaseContext!.PersonsTable.ToList().Count > 0;

        public List<Person> PersonsList
        {
            get => persList;
            set
            {
                persList = value;
                OnPropertyChanged(nameof(PersonsList));
            }
        }
        public List<Note> NotesList
        {
            get => notlst;
            set
            {
                notlst = value;
                OnPropertyChanged(nameof(NotesList));
            }
        }
        public List<Place> PlacesList
        {
            get => placelst;
            set
            {
                placelst = value;
                OnPropertyChanged(nameof(PlacesList));
            }
        }

        public List<Rod> RodsList
        {
            get => rodlst;
            set
            {
                rodlst = value;
                OnPropertyChanged(nameof(RodsList));
            }
        }        

        private List<Note> notlst;
        private List<Rod> rodlst;
        private List<Place> placelst;
        private List<Person> persList;
        private Tree? currentTree;
        private Person? currentPerson;
        private bool isTreeCrOrOp;

        #region Commands
        public ICommand NewTreeCommand => new RelayCommand(CreateNewTree);
        public ICommand OpenTreeCommand => new RelayCommand(OpenTree);
        public ICommand SaveTreeCommand => new RelayCommand(SaveTree);

        #region Persons
        public ICommand AddValueCommand => new RelayCommand(Add);
        public ICommand DeleteValueCommand => new RelayCommand(Delete);
        public ICommand EditValueCommand => new RelayCommand(Edit);
        #endregion
        #endregion

        #region CreateOrOpenTreeMethods
        private void OpenTree(object obj)
        {
            CreateTreeView treeview = new CreateTreeView(true);
            if (treeview.ShowDialog() == true)
            {
                CurrentTree = StaticValues.AddTree;
                IsTreeCreatedOrOpen = true;
                LoadValuesAfterGetTree();
            }
        }

        private void CreateNewTree(object obj)
        {
            CreateTreeView treeview = new CreateTreeView();
            if (treeview.ShowDialog() == true)
            {
                CurrentTree = StaticValues.AddTree;
                LoadValuesAfterGetTree();
                IsTreeCreatedOrOpen = true;
            }
        }

        #endregion

        private void LoadValuesAfterGetTree()
        {
            //PersonsList = App.DatabaseContext.PersonsTable.Select("SELECT * FROM Persons WHERE ");
            PlacesList = App.DatabaseContext.PlaceTable.ToList();
            NotesList = App.DatabaseContext.NoteTable.Select($"SELECT * FROM notes WHERE TreeID = {CurrentTree!.ID};");
            RodsList = App.DatabaseContext.RodsTable.Select($"SELECT * FROM rods WHERE TreeID ={CurrentTree!.ID};");
        }

        private void SaveTree(object obj)
        {
            IsTreeCreatedOrOpen = false;
            MessageBox.Show("Изменения сохранены", "Сохранение дерева",
                MessageBoxButton.OK, MessageBoxImage.Information);
            IsTreeCreatedOrOpen = true;
        }

        private void Add(object obj)
        {
            if (obj is TabItem == false) return;
            TabItem tab = (TabItem)obj;
            switch (tab.Header)
            {
                case "Персоны":
                    {
                        EditPersonView add = new EditPersonView();
                        add.ShowDialog();
                        PersonsList = App.DatabaseContext.PersonsTable.ToList();
                        break;
                    }
                case "Заметки":
                    {
                        EditNoteView add = new EditNoteView(CurrentTree);
                        add.ShowDialog();
                        NotesList = App.DatabaseContext.NoteTable.Select($"SELECT * FROM notes WHERE TreeID = {CurrentTree!.ID};");
                        break;
                    }
                case "Места":
                    {
                        EditPlaceView add = new EditPlaceView();
                        add.ShowDialog();
                        PlacesList = App.DatabaseContext.PlaceTable.ToList();
                        break;
                    }
                case "Роды":
                    {
                        EditRodView add = new EditRodView(CurrentTree);
                        add.ShowDialog();
                        RodsList = App.DatabaseContext.RodsTable.Select($"SELECT * FROM rods WHERE TreeID ={CurrentTree!.ID};");
                        break;
                    }
            }
        }

        private void Edit(object obj)
        {
            if (obj is TabItem == false) return;
            TabItem tab = (TabItem)obj;
            switch (tab.Header)
            {
                case "Персоны":
                    {
                        if (SelectedPerson == null) return;
                        EditPersonView add = new EditPersonView(SelectedPerson);
                        add.ShowDialog();
                        if (AddEditPerson != null)
                        {
                            PersonsList = App.DatabaseContext!.PersonsTable.ToList();
                        }
                        break;
                    }
                case "Заметки":
                    {
                        if (SelectedNote == null) return;
                        EditNoteView add = new EditNoteView(currentTree, SelectedNote);
                        add.ShowDialog();
                        NotesList = App.DatabaseContext.NoteTable.Select($"SELECT * FROM notes WHERE TreeID = {CurrentTree!.ID};");
                        break;
                    }
                case "Места":
                    {
                        if (SelectedPlace == null) return;
                        EditPlaceView add = new EditPlaceView(SelectedPlace);
                        add.ShowDialog();
                        PlacesList = App.DatabaseContext.PlaceTable.ToList();
                        break;
                    }
                case "Роды":
                    {
                        if (SelectedRod == null) return;
                        EditRodView add = new EditRodView(CurrentTree, SelectedRod);
                        add.ShowDialog();
                        RodsList = App.DatabaseContext.RodsTable.Select($"SELECT * FROM rods WHERE TreeID ={CurrentTree!.ID};");
                        break;
                    }
            }
        }

        private void Delete(object obj)
        {
            if (obj is TabItem == false) return;
            TabItem tab = (TabItem)obj;
            switch (tab.Header)
            {
                case "Персоны":
                    {
                        if (SelectedPerson == null) return;
                        App.DatabaseContext!.PersonsTable.Remove(SelectedPerson);
                        PersonsList = App.DatabaseContext!.PersonsTable.ToList();
                        MessageBox.Show("Выбранная персона удалена!", "Удаление персоны", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                case "Заметки":
                    {
                        if (SelectedNote == null) return;
                        App.DatabaseContext!.NoteTable.Remove(SelectedNote);
                        NotesList = App.DatabaseContext.NoteTable.Select($"SELECT * FROM notes WHERE TreeID = {CurrentTree!.ID};");
                        MessageBox.Show("Выбранная заметка удалена!", "Удаление заметки", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                case "Места":
                    {
                        if (SelectedPlace == null) return;
                        App.DatabaseContext!.PlaceTable.Remove(SelectedPlace);
                        PlacesList = App.DatabaseContext.PlaceTable.ToList();
                        MessageBox.Show("Выбранное место удалено!", "Удаление места", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                case "Роды":
                    {
                        if (SelectedRod == null) return;
                        App.DatabaseContext!.RodsTable.Remove(SelectedRod);
                        RodsList = App.DatabaseContext.RodsTable.Select($"SELECT * FROM rods WHERE TreeID = {CurrentTree!.ID};");
                        MessageBox.Show("Выбранный род удален!", "Удаление рода", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
            }
        }
    }
}