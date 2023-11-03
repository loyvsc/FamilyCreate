using FamilyCreate.Models;
using FamilyCreate.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace FamilyCreate.ViewModels
{
    public class EditSourceViewModel : NotifyPropertyChangedBase
    {
        public Source Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }
        public string? Title
        {
            get => ttl;
            set
            {
                ttl = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string? OKButtonText
        {
            get => okbuttext;
            set
            {
                okbuttext = value;
                OnPropertyChanged(nameof(OKButtonText));
            }
        }
        public ICommand OKButtonCommand => new RelayCommand(OKButton);
        public ICommand CancelCommand => new RelayCommand((object obj) => parent.DialogResult = true);

        #region Private Vars
        private string? okbuttext;
        private string? ttl;
        private readonly Tree tree;
        private Source source;
        private readonly EditSourceView parent;
        #endregion

        #region Constructors
        public EditSourceViewModel()
        {
            Title = "Добавление источника";
            Source = new Source();
        }

        public EditSourceViewModel(EditSourceView parent, Tree tree) : this()
        {
            this.tree = tree;
            this.parent = parent;
        }

        public EditSourceViewModel(EditSourceView parent, Tree tree, Source source)
        {
            Title = "Редактирование источника";
            this.tree = tree;
            this.parent = parent;
            Source = source;
        }
        #endregion

        private void OKButton(object obj)
        {
            if (!Source.IsValid)
            {
                MessageBox.Show("Введите название источника!", "Добавление источника", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Source.ID != -1) //если источник добавлен
            {
                App.DatabaseContext.SourceTable.Update(Source);
            }
            else
            {
                Source.TreeID = tree.ID;
                App.DatabaseContext.SourceTable.Add(Source);
            }
            parent.DialogResult = true;
        }
    }
}