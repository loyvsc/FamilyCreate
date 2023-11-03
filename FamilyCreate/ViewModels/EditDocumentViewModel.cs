using FamilyCreate.Models;
using FamilyCreate.Views;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class EditDocumentViewModel : NotifyPropertyChangedBase
    {
        public Document Document
        {
            get => doc;
            set
            {
                doc = value;
                OnPropertyChanged(nameof(Document));
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
        public Models.File File
        {
            get => file;
            set
            {
                file = value;
                OnPropertyChanged(nameof(File));
            }
        }

        public string AddDateText => Document.ID != -1 ? Document.AddDate.ToShortDateString() : "";
        public List<Source> SourceList
        {
            get => srclst;
            set
            {
                srclst = value;
                OnPropertyChanged(nameof(SourceList));
            }
        }

        public int SelectedSourceIndex
        {
            get => selsrcind;
            set
            {
                selsrcind = value;
                OnPropertyChanged(nameof(SelectedSourceIndex));
            }
        }

        #region Commands
        public ICommand OKButtonCommand => new RelayCommand(OKButton);
        public ICommand CancelCommand => new RelayCommand((object obj) => parent.DialogResult = true);
        public ICommand LoadFileCommand => new RelayCommand(LoadFile);
        public ICommand SaveFileCommand => new RelayCommand(SaveFile);
        #endregion

        #region Private Vars
        private List<Source> srclst;
        private int selsrcind;
        private Models.File file;
        private string? okbuttext;
        private string? ttl;
        private readonly Tree tree;
        private Document doc;
        private readonly EditDocumentView parent;
        #endregion

        #region Constructors
        public EditDocumentViewModel()
        {
            File = new Models.File();
            Title = "Добавление документа";
            Document = new Document()
            {
                AddDate = System.DateTime.Now
            };
            SelectedSourceIndex = -1;
        }

        public EditDocumentViewModel(EditDocumentView parent, Tree tree) : this()
        {
            this.tree = tree;
            this.parent = parent;
            SourceList = App.DatabaseContext.SourceTable.Select($"SELECT * FROM SOURCES WHERE TREEID = {tree.ID};");
        }

        public EditDocumentViewModel(EditDocumentView parent, Tree tree, Document document)
        {
            Title = "Редактирование документа";
            this.tree = tree;
            this.parent = parent;
            Document = document;
            File = Document.File;
            SourceList = App.DatabaseContext.SourceTable.Select($"SELECT * FROM SOURCES WHERE TREEID = {tree.ID};");
            var bufsrc = App.DatabaseContext.SourceTable.ElementAt(Document.SourceID);
            SelectedSourceIndex = SourceList.IndexOf(SourceList.Where((x) => x.ID == bufsrc.ID).ToArray()[0]);
        }
        #endregion

        private void LoadFile(object obj)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                string filename = System.IO.Path.GetFileName(open.FileName);
                byte[] data = System.IO.File.ReadAllBytes(open.FileName);
                string extension = System.IO.Path.GetExtension(open.FileName);
                SetFile(filename, data, extension);
            }
        }

        private void SaveFile(object obj)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                Filter = $"Изначальный формат (*{File.Extension})|*{File.Extension}|Все файлы (*.*)|*.*"
            };
            if (save.ShowDialog() == true)
            {
                string savePath = save.FileName;

                FileInfo fi1 = new FileInfo(savePath);

                if (!fi1.Exists)
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(savePath, FileMode.OpenOrCreate))
                    {
                        fs.Write(File.FileText, 0, File.FileText.Length);
                    }
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(savePath);
                    }
                    catch
                    {
                        MessageBox.Show("Файл заблокирован других процессом!", "Сохранение файла", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    using (StreamWriter sw = fi1.CreateText())
                    {
                        sw.Write(File.FileText);
                    }
                }

                MessageBox.Show("Файл успешно сохранен!", "Сохранение файла", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SetFile(string filename, byte[] data, string extension)
        {
            File.FileText = data;
            File.Extension = extension;
            File.Name = filename;
        }

        private void OKButton(object obj)
        {
            if (!Document.IsValid)
            {
                MessageBox.Show("Введите всю информацию о документе!", "Добавление документа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Document.ID != -1)
            {
                CheckFile();
                Document.FileID = App.DatabaseContext.FileTable.Select("SELECT * FROM FILES WHERE ID = (SELECT MAX(ID) FROM FILES);")[0].ID;
                App.DatabaseContext.DocumentTable.Update(Document);
            }
            else
            {
                Document.TreeID = tree.ID;
                CheckFile();
                Document.SourceID = SourceList[SelectedSourceIndex].ID;
                App.DatabaseContext.DocumentTable.Add(Document);
            }
            parent.DialogResult = true;
        }

        private void CheckFile()
        {
            if (Document.FileID != -1)
            {
                if (Document.File.Name != File.Name ||
                    Document.File.FileText != File.FileText ||
                    Document.File.Extension != File.Extension)
                {
                    App.DatabaseContext.FileTable.Update(File);
                }
            }
            else
            {
                try
                {
                    App.DatabaseContext.FileTable.Add(File);
                    Document.FileID = App.DatabaseContext.FileTable.Select("SELECT * FROM FILES WHERE ID = (SELECT MAX(ID) FROM FILES);")[0].ID;
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении файла!\nПовторите попытку позже...", "Добавление файла", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}