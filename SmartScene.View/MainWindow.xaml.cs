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
using MahApps.Metro.Controls;
using SmartScene.ViewModel;

namespace SmartScene.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        AppFrameworkVM _appFrameworkVM = new AppFrameworkVM();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _appFrameworkVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _appFrameworkVM.MainTabPanels.Add(new MainTabPanel() { Title="sdfsdfas"});
        }
    }
}
