using FamilyCreate.Models;
using System;
using System.Windows;

namespace FamilyCreate.Views
{
    /// <summary>
    /// Логика взаимодействия для TreeViewerView.xaml
    /// </summary>
    public partial class TreeViewerView : Window,IDisposable
    {
        public TreeViewerView(Tree tree)
        {
            InitializeComponent();
            this.DataContext = new FamilyCreate.ViewModels.TreeViewerViewModel(this, tree);
        }

        public void Dispose()
        {
            Area.Dispose();
        }
    }
}
