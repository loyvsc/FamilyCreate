using FamilyCreate.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class CreateTreeViewModel : NotifyPropertyChangedBase
    {
        public Tree? NewTree
        {
            get => newTree;
            set
            {
                newTree = value;
                OnPropertyChanged(nameof(NewTree));
            }
        }
        public ICommand ButtonCommand
        {
            get => butCom;
            set
            {
                butCom = value;
                OnPropertyChanged(nameof(ButtonCommand));
            }
        }

        public string? ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }
        public string? DescriptionText
        {
            get => descTxt;
            set
            {
                descTxt = value;
                OnPropertyChanged(nameof(DescriptionText));
            }
        }
        public List<Tree>? Trees
        {
            get => trees;
            set
            {
                trees = value;
                OnPropertyChanged(nameof(Trees));
            }
        }

        private List<Tree>? trees;
        private string? descTxt;
        private ICommand butCom;
        private string? buttonText;
        private Tree? newTree;
        private readonly Window? parentWindow;

        public CreateTreeViewModel() => NewTree = new Tree();

        public CreateTreeViewModel(Window parent, Tree? foredit = null)
        {
            parentWindow = parent;
            if (foredit != null)
            {
                NewTree = foredit;
                ButtonCommand = new RelayCommand(SelectCreatedTree);
                Trees = App.DatabaseContext.TreeTable.ToList();
                parentWindow.Title = "Редактирование дерева";
                ButtonText = "Сохранить";
                DescriptionText = "Введите название дерева";
            }
            else
            {
                ButtonCommand = new RelayCommand(CreateNewTree);
                parentWindow.Title = "Создание дерева";
                DescriptionText = "Введите название дерева";
                ButtonText = "Создать дерево";
                NewTree = new Tree();
            }
        }

        private void CreateNewTree(object obj)
        {
            if (NewTree!.IsValid)
            {
                MessageBox.Show("Введите название дерева!", "Создание дерева", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                App.DatabaseContext.TreeTable.Add(NewTree);
                parentWindow!.DialogResult = true;
            }
        }

        private void SelectCreatedTree(object obj)
        {
            if (NewTree!.IsValid)
            {
                App.DatabaseContext.TreeTable.Update(NewTree);
                parentWindow!.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите новое название дерева!", "Редактирование дерева", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}