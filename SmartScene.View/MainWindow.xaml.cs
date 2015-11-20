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
using ESRI.ArcGIS.Client;
using SmartScene.ViewModel.Tab;
using SmartScene.ViewModel.Map;

namespace SmartScene.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow 
    {
        
        MainWindowVM _mainWindowVM = new MainWindowVM();
        public MainWindow()
        {
            InitializeComponent();
            ViewModelContext.Current.ThemeManagement = new ThemeManagement();
            ViewModelContext.Current.TabContentViewProvider = new TabContentViewProvider();
            this.DataContext = _mainWindowVM;
        }

        public MainWindowVM MainWindowVM
        {
            get
            {
                return _mainWindowVM;
            }

          
        }
        int i;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            i++;
            SceneVM scene = new SceneVM();
            scene.SelectedMapVM = new ViewModel.Map.MapVM();
            scene.SelectedMapVM.LayerCollection.Add(new SingleTileLayer() { TileImageURI = AppDomain.CurrentDomain.BaseDirectory + @"map.PNG", MaximumResolution = 10, MinimumResolution = 0.1 });
            TabPanelVM vm = new TabPanelVM() { Title = "警卫任务" + i.ToString(), Content = scene };
            _mainWindowVM.TabPanelManagerVM.DefaultTabControlVM.TabPanelVMs.Add(vm);
            _mainWindowVM.TabPanelManagerVM.DefaultTabControlVM.SelectedTabPanelVM = vm;
        }




        double leftSideWidth = 280.0;
        private void expender_left_Collapsed(object sender, RoutedEventArgs e)
        {
            leftSideWidth = this.leftSide.ActualWidth;
            this.leftSide.Width = new GridLength(0, GridUnitType.Auto);
        }

        private void expender_left_Expanded(object sender, RoutedEventArgs e)
        {
            if (this.leftSideWidth < 46) this.leftSideWidth = 64;
            this.leftSide.Width = new GridLength(this.leftSideWidth, GridUnitType.Pixel);
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if(!expender_left.IsExpanded)
            {
                this.leftSide.Width = new GridLength(0, GridUnitType.Auto);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SceneVM scene = new SceneVM();
            scene.SelectedMapVM = new ViewModel.Map.MapVM();
            //scene.SelectedMapVM.LayerCollection.Add(new SingleTileLayer() { TileImageURI = AppDomain.CurrentDomain.BaseDirectory + @"map.PNG", MaximumResolution = 10, MinimumResolution = 0.1 });
            //TabPanelVM vm = new TabPanelVM() { Title = "总指挥屏", Content = scene };
            scene.SelectedMapVM.LayerCollection.Add(new ArcGISTiledMapServiceLayer() { Url = @"http://169.254.80.80:7080/PBS/rest/services/MyPBSService1/MapServer" });
            TabPanelVM vm = new TabPanelVM() { Title = "总指挥屏", Content = scene };
            vm.CanClose = false;
            _mainWindowVM.TabPanelManagerVM.DefaultTabControlVM.TabPanelVMs.Add(vm);
            _mainWindowVM.TabPanelManagerVM.DefaultTabControlVM.SelectedTabPanelVM = vm;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Flyout_ConflictEdit.IsOpen = !this.Flyout_ConflictEdit.IsOpen;
        }
    }
}
