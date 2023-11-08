using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Collections.Generic;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class SelectTreeViewModel : NotifyPropertyChangedBase
    {
        public List<Tree> TreesList
        {
            get => trees;
            set
            {
                trees = value;
                OnPropertyChanged(nameof(TreesList));
            }
        }
        public Tree? SelectedTree
        {
            get => seltree;
            set
            {
                seltree = value;
                OnPropertyChanged(nameof(SelectedTree));
            }
        }

        #region Private vars
        private List<Tree> trees;
        private Tree? seltree;
        private readonly SelectTreeView parent;
        #endregion

        #region Constructors
        public SelectTreeViewModel()
        {
            TreesList = App.DatabaseContext.TreeTable.ToList();
        }
        public SelectTreeViewModel(SelectTreeView view) : this()
        {
            parent = view;
        }
        #endregion

        public ICommand AddCommand => new RelayCommand(Add);
        public ICommand EditCommand => new RelayCommand(Edit);
        public ICommand RemoveCommand => new RelayCommand(Delete);
        public ICommand CloseCommand => new RelayCommand(Close);
        public ICommand SelectCommand => new RelayCommand(Select);

        private void Add(object obj)
        {
            CreateTreeView create = new CreateTreeView();
            if (create.ShowDialog() == true)
            {
                TreesList = App.DatabaseContext.TreeTable.ToList();
            }
        }

        private void Edit(object obj)
        {
            CreateTreeView create = new CreateTreeView(SelectedTree);
            if (create.ShowDialog() == true)
            {
                TreesList = App.DatabaseContext.TreeTable.ToList();
            }
        }

        private void Delete(object obj)
        {
            if (SelectedTree != null)
            {
                App.DatabaseContext.TreeTable.Remove(SelectedTree);
                TreesList = App.DatabaseContext.TreeTable.ToList();
            }
        }

        private void Select(object obj)
        {
            if (SelectedTree == null) return;
            StaticValues.AddTree = SelectedTree;
            parent.DialogResult = true;
        }

        private void Close(object obj) => parent.DialogResult = false;
    }
}
