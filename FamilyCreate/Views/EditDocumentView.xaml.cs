using FamilyCreate.Models;
using FamilyCreate.ViewModels;
using System;
using System.IO;
using System.Windows;

namespace FamilyCreate.Views
{
    public partial class EditDocumentView : Window
    {
        public EditDocumentView(Tree currentTree)
        {
            InitializeComponent();
            this.DataContext = new EditDocumentViewModel(this, currentTree);
        }
        public EditDocumentView(Tree currentTree, Document doc)
        {
            InitializeComponent();
            this.DataContext = new EditDocumentViewModel(this, currentTree, doc);
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            string filename = GetFilename(e);
            byte[] data = System.IO.File.ReadAllBytes(filename);
            string extension = Path.GetExtension(filename);

            ((EditDocumentViewModel)DataContext).SetFile(filename, data, extension);
        }

        private string GetFilename(DragEventArgs e)
        {
            string filename = string.Empty;

            if ((e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is string))
                    {
                        filename = ((string[])data)[0];
                    }
                }
            }
            return filename;
        }
    }
}