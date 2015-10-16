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
        
        MainWindowVM _mainWindowVM = new MainWindowVM();
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowVM.ThemeManagement = new ThemeManagement();
            this.DataContext = _mainWindowVM;
        }

        public MainWindowVM MainWindowVM
        {
            get
            {
                return _mainWindowVM;
            }

          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowVM.TabControlVMs[0].TabPanelVMs.Add(new TabPanelVM() { Title="标签页面首页", Content = DateTime.Now.ToString() });
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //this.expender_left.Width = this.expender_left.ActualWidth ;
            this.expender_left.Width = double.NaN;
            this.leftSide.Width= new GridLength(0,GridUnitType.Auto);
            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
