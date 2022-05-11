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
    /// Логика взаимодействия для BackButton.xaml
    /// </summary>
    public partial class BackButton : UserControl
    {
        public FolderInfo ParentInfo { get; set; }
        public Action<FolderInfo> UpdateFolders { get; set; }

        public BackButton(FolderInfo parentInfo, Action<FolderInfo> updateFolders)
        {
            InitializeComponent();
            ParentInfo = parentInfo;
            UpdateFolders = updateFolders;
            BackBtn.Click += SwitchBackToParent;
        }

        public void SwitchBackToParent(object sender, RoutedEventArgs e)
        {
            if (ParentInfo == null)
                throw new InvalidOperationException();
            UpdateFolders.Invoke(ParentInfo);
        }
    }
}
