using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaction logic for FileListControl.xaml
    /// </summary>
    public partial class FileListControl : UserControl
    {
        public readonly Brush TextColor = Brushes.LightSlateGray;

        public FileListControl()
        {
            InitializeComponent();
        }

        public void BrowseFolder()
        {
            var folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

            // Show the FolderBrowserDialog.
            System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                var folderTree = GetFolderTree(folderName);
                UpdateFolders(folderTree);
                //AddFoldersPanel(folderTree);
                //Do your work here!
            }            
        }

        public void UpdateFolders(FolderInfo folder)
        {
            ButtonsPanel.Children.Clear();
            if (folder.ParentFolder != null)
                ButtonsPanel.Children.Add(new BackButton(folder.ParentFolder, UpdateFolders));
            foreach(var fldr in folder.InnerFolders)
                ButtonsPanel.Children.Add(new Folder(fldr, UpdateFolders));
        }

        public FolderInfo GetFolderTree(string rootFolderPath)
        {
            var directory = new DirectoryInfo(rootFolderPath);
            var result = new FolderInfo(directory.FullName, directory.Name, directory.LastWriteTime);
            DirectoryInfo[] directories = null;
            try
            {
                directories = directory.GetDirectories();
            }
            catch
            {
                return result;
            }
            foreach(var dir in directories)
                result.AddInnerFolder(GetFolderTree(dir.FullName));
            return result;
        }

        private void AddFoldersPanel(FolderInfo rootFolder)
        {
            foreach(var folder in rootFolder.InnerFolders)
            {
                var btn = GetFolderButton(new FolderInfoViewModel(folder.Name, 0, folder.LastModified, 0));
                ButtonsPanel.Children.Add(btn);
            }
        }

        private Button GetFolderButton(FolderInfoViewModel folderInfo)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) }); 
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.Children.Add(GetFolderIconWithName(folderInfo.Name));
            grid.Children.Add(GetFolderDescriptionTextBlock(folderInfo.ItemsCount + " items", 1));
            grid.Children.Add(GetFolderDescriptionTextBlock(folderInfo.LastModified.ToString("mm d, yyyy"), 2));
            grid.Children.Add(GetFolderDescriptionTextBlock(folderInfo.FolderSize + " bytes", 3));
            var btn = new Button
            {
                Content = grid
            };
            Style style = this.FindResource("ButtonStyle1") as Style;
            btn.Style = style;
            return btn;
        }

        private StackPanel GetFolderIconWithName(string name)
        {
            var panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Stretch;
            panel.VerticalAlignment = VerticalAlignment.Stretch;
            panel.Children.Add(new Image 
            { 
                Margin = new Thickness(5, 0, 0, 0), 
                Width = 50, 
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Left,
                Source = new BitmapImage(
                    new Uri("/assets/folder.png", UriKind.Relative))
            });
            panel.Children.Add(new TextBlock
            {
                Margin = new Thickness(5, 0, 0, 0),
                Text = name,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold,
                Foreground = TextColor
            });
            Grid.SetColumn(panel, 0);
            return panel;
        }

        private TextBlock GetFolderDescriptionTextBlock(string text, int col)
        {
            var block = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = TextColor
            };
            Grid.SetColumn(block, col);
            return block;
        }
    }

    public class FolderInfoViewModel
    {
        public string Name { get; set; }
        public int ItemsCount { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public ulong FolderSize { get; set; }

        public FolderInfoViewModel(string name, int count, DateTimeOffset lastModified, ulong folderSize)
        {
            Name = name;
            ItemsCount = count;
            LastModified = lastModified;
            FolderSize = folderSize;
        }
    }

    public class FolderInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public FolderInfo ParentFolder { get; set; }
        public List<FolderInfo> InnerFolders { get; set; }
        public FolderInfo(string path, string name, DateTimeOffset lastModified)
        {
            this.Path = path;
            this.Name = name;
            this.LastModified = lastModified;
            InnerFolders = new List<FolderInfo>();
        }

        public void AddInnerFolder(FolderInfo folderInfo)
        {
            InnerFolders.Add(folderInfo);
            folderInfo.ParentFolder = this;
        }
    }
}
