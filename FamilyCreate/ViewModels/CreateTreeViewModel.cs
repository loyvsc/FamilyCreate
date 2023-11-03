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

        public Visibility IsOpenBoxEnabled
        {
            get => isOpenBoxEnabled;
            set
            {
                isOpenBoxEnabled = value;
                OnPropertyChanged(nameof(IsOpenBoxEnabled));
            }
        }

        public Visibility IsCreateBoxEnabled
        {
            get => isCreateBoxEnabled;
            set
            {
                isCreateBoxEnabled = value;
                OnPropertyChanged(nameof(IsCreateBoxEnabled));
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

        public string? TitleText
        {
            get => titleText;
            set
            {
                titleText = value;
                OnPropertyChanged(nameof(TitleText));
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
        private string? titleText;
        private string? buttonText;
        private Tree? newTree;
        private Visibility isCreateBoxEnabled;
        private Visibility isOpenBoxEnabled;
        private readonly Window? parentWindow;

        public CreateTreeViewModel() => NewTree = new Tree();

        public CreateTreeViewModel(Window parent, bool forOpen = false)
        {
            parentWindow = parent;
            if (forOpen)
            {
                ButtonCommand = new RelayCommand(SelectCreatedTree);
                Trees = App.DatabaseContext.TreeTable.ToList();
                TitleText = "Открытие дерева";
                ButtonText = "Открыть дерево";
                DescriptionText = "Выберите дерево";
                IsCreateBoxEnabled = Visibility.Collapsed;
                IsOpenBoxEnabled = Visibility.Visible;
            }
            else
            {
                ButtonCommand = new RelayCommand(CreateNewTree);
                TitleText = "Создание нового дерева";
                DescriptionText = "Название нового дерева";
                ButtonText = "Создать дерево";
                IsCreateBoxEnabled = Visibility.Visible;
                IsOpenBoxEnabled = Visibility.Collapsed;
                NewTree = new Tree();
            }
        }

        private void CreateNewTree(object obj)
        {
            if (NewTree!.Name == string.Empty)
            {
                MessageBox.Show("Введите название дерева!", "Создание дерева", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                App.DatabaseContext?.TreeTable.Add(NewTree);
                NewTree.ID = App.DatabaseContext!.TreeTable.GetIDByItem(NewTree);
                StaticValues.AddTree = newTree;
                parentWindow!.DialogResult = true;
            }
        }

        private void SelectCreatedTree(object obj)
        {
            if (NewTree == null)
            {
                MessageBox.Show("Выберите дерево из списка!", "Открытие дерева", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                StaticValues.AddTree = NewTree;
                parentWindow!.DialogResult = true;
            }
        }
    }
}