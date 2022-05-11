using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManagerUI.Custom_Controls
{
    /// <summary>
    /// Логика взаимодействия для Folder.xaml
    /// </summary>
    public partial class Folder : UserControl
    {
        public string FolderName { get; set; }
        public string ItemsCount { get; set; }
        public string LastModified { get; set; }
        public string FolderSize { get; set; }
        public FolderInfo FolderInfo { get; set; }
        public Action<string> UpdateFolders { get; set; }

        public Folder(FolderInfo folderInfo, Action<string> updateFolders)
        {
            InitializeComponent();
            FolderName = folderInfo.Name;
            ItemsCount = folderInfo.InnerFolders.Count.ToString();
            LastModified = folderInfo.LastModified.ToString("mm d, yyyy");
            FolderSize = "1 Gb";
            FolderInfo = folderInfo;
            UpdateFolders = updateFolders;
            FolderButton.Click += SwitchToFolder;

            this.DataContext = this;
        }

        public void SwitchToFolder(object sender, RoutedEventArgs e)
        {
            UpdateFolders.Invoke(FolderInfo.Path);
        }
    }
}
