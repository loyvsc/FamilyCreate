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
        public Tree CurrentTree
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
        public Event? SelectedEvent
        {
            get => selevnt;
            set
            {
                selevnt = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }
        public Source? SelectedSource
        {
            get => selsrc;
            set
            {
                selsrc = value;
                OnPropertyChanged(nameof(SelectedSource));
            }
        }
        public Place? SelectedPlace
        {
            get => selplc;
            set
            {
                selplc = value;
                OnPropertyChanged(nameof(SelectedPlace));
            }
        }
        public Document? SelectedDocument
        {
            get => seldoc;
            set
            {
                seldoc = value;
                OnPropertyChanged(nameof(SelectedDocument));
            }
        }

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

        public bool IsSelectedPersonNotNull => SelectedPerson != null;
        public bool IsCurrentTreeSelected => CurrentTree != null;
        public bool IsPrintEnabled => App.DatabaseContext!.PersonsTable.ToList().Count > 0;

        #region Lists
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
        public List<Event> EventList
        {
            get => evtlst;
            set
            {
                evtlst = value;
                OnPropertyChanged(nameof(EventList));
            }
        }
        public List<Source> SourceList
        {
            get => srclst;
            set
            {
                srclst = value;
                OnPropertyChanged(nameof(SourceList));
            }
        }
        public List<Document> DocumentsList
        {
            get => doclst;
            set
            {
                doclst = value;
                OnPropertyChanged(nameof(DocumentsList));
            }
        }
        #endregion

        #region Private Values
        private Place selplc;
        private List<Document> doclst;
        private List<Event> evtlst;
        private List<Source> srclst;
        private List<Note> notlst;
        private List<Rod> rodlst;
        private List<Place> placelst;
        private List<Person> persList;
        private Tree currentTree;
        private Person? currentPerson;
        private bool isTreeCrOrOp;
        private Source? selsrc;
        private Event? selevnt;
        private Rod? selrod;
        private Note? selnot;
        private Document? seldoc;
        #endregion

        #region Commands
        public ICommand NewTreeCommand => new RelayCommand(CreateNewTree);
        public ICommand OpenTreeCommand => new RelayCommand(OpenTree);
        public ICommand SaveTreeCommand => new RelayCommand(SaveTree);

        public ICommand CreateTreeCommand => new RelayCommand(CreateTree);
        public ICommand AddValueCommand => new RelayCommand(Add);
        public ICommand DeleteValueCommand => new RelayCommand(Delete);
        public ICommand EditValueCommand => new RelayCommand(Edit);
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
            PersonsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM Persons WHERE Rodid in (SELECT ID FROM RODS WHERE TREEID = {CurrentTree.ID});");
            PlacesList = App.DatabaseContext.PlaceTable.ToList();
            NotesList = App.DatabaseContext.NoteTable.Select($"SELECT * FROM notes WHERE TreeID = {CurrentTree.ID};");
            RodsList = App.DatabaseContext.RodsTable.Select($"SELECT * FROM rods WHERE TreeID ={CurrentTree.ID};");
            EventList = App.DatabaseContext.EventTable.Select($"SELECT * FROM EVENTS WHERE RODID IN (SELECT ID FROM rods WHERE TreeID ={CurrentTree.ID});");
            SourceList = App.DatabaseContext.SourceTable.Select($"SELECT * FROM SOURCES WHERE TREEID = {CurrentTree.ID};");
            DocumentsList = App.DatabaseContext.DocumentTable.Select($"SELECT * FROM DOCUMENTS WHERE TREEID = {CurrentTree.ID};");
        }

        private void CreateTree(object obj)
        {
            TreeViewerView tree = new TreeViewerView(CurrentTree);
            tree.ShowDialog();
            PersonsList = App.DatabaseContext.PersonsTable.ToList();
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
                        PersonsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM Persons WHERE Rodid in (SELECT ID FROM RODS WHERE TREEID = {CurrentTree!.ID});");
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
                case "События":
                    {
                        EditEventView add = new EditEventView(CurrentTree);
                        add.ShowDialog();
                        EventList = App.DatabaseContext.EventTable.Select($"SELECT * FROM EVENTS WHERE RODID IN (SELECT ID FROM RODS WHERE TREEID ={CurrentTree!.ID});");
                        break;
                    }
                case "Источники":
                    {
                        EditSourceView add = new EditSourceView(CurrentTree);
                        add.ShowDialog();
                        SourceList = App.DatabaseContext.SourceTable.Select($"SELECT * FROM SOURCES WHERE TREEID = {CurrentTree.ID};");
                        break;
                    }
                case "Документы":
                    {
                        EditDocumentView add = new EditDocumentView(CurrentTree);
                        add.ShowDialog();
                        DocumentsList = App.DatabaseContext.DocumentTable.Select($"SELECT * FROM documents WHERE TREEID = {CurrentTree.ID};");
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
                        PersonsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM Persons WHERE Rodid in (SELECT ID FROM RODS WHERE TREEID = {CurrentTree!.ID});");
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
                case "События":
                    {
                        if (SelectedEvent == null) return;
                        EditEventView add = new EditEventView(CurrentTree, SelectedEvent);
                        add.ShowDialog();
                        EventList = App.DatabaseContext.EventTable.Select($"SELECT * FROM EVENTS WHERE RODID IN (SELECT ID FROM RODS WHERE TREEID ={CurrentTree!.ID});");
                        break;
                    }
                case "Документы":
                    {
                        if (SelectedDocument == null) return;
                        EditDocumentView add = new EditDocumentView(CurrentTree, SelectedDocument);
                        add.ShowDialog();
                        DocumentsList = App.DatabaseContext.DocumentTable.Select($"SELECT * FROM DOCUMENTS WHERE TREEID = {CurrentTree.ID};");
                        break;
                    }
                case "Источники":
                    {
                        if (SelectedSource == null) return;
                        EditSourceView add = new EditSourceView(CurrentTree, SelectedSource);
                        add.ShowDialog();
                        SourceList = App.DatabaseContext.SourceTable.Select($"SELECT * FROM SOURCES WHERE TREEID = {CurrentTree.ID};");
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
                        PersonsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM Persons WHERE Rodid in (SELECT ID FROM RODS WHERE TREEID = {CurrentTree!.ID});");
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
                case "События":
                    {
                        if (SelectedEvent == null) return;
                        App.DatabaseContext.EventTable.Remove(SelectedEvent);
                        EventList = App.DatabaseContext.EventTable.Select($"SELECT * FROM EVENTS WHERE RODID in (SELECT ID FROM RODS WHERE TREEID = {CurrentTree!.ID})");
                        MessageBox.Show("Выбранное событие удален!", "Удаление события", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                case "Источники":
                    {
                        if (SelectedSource == null) return;
                        App.DatabaseContext.SourceTable.Remove(SelectedSource);
                        SourceList = App.DatabaseContext.SourceTable.Select($"SELECT * FROM SOURCES WHERE TREEID = {CurrentTree.ID};");
                        MessageBox.Show("Выбранный источник удален!", "Удаление источника", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                case "Документы":
                    {
                        if (SelectedDocument == null) return;
                        App.DatabaseContext.DocumentTable.Remove(SelectedDocument);
                        DocumentsList = App.DatabaseContext.DocumentTable.Select($"SELECT * FROM DOCUMENTS WHERE TREEID = {CurrentTree.ID};");
                        MessageBox.Show("Выбранный документ удален!", "Удаление документа", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
            }
        }
    }
}