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
using ESRI.ArcGIS.Client;
using SmartScene.ViewModel.Map;

namespace SmartScene.View
{
    /// <summary>
    /// MapView.xaml 的交互逻辑
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
        }
        Map _map;
        public static readonly DependencyProperty MapVMProperty = DependencyProperty.Register("MapVM", typeof(MapVM), typeof(MapView), new PropertyMetadata(null, new PropertyChangedCallback(MapVMPropertyChangedCallback)));
        public MapVM MapVM
        {
            get {return(MapVM) base.GetValue(MapVMProperty); }
            set { SetValue(MapVMProperty, value); }
        }
        private static void MapVMPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mv = (MapView)d;
            mv.grid_Content.Children.Clear();
            MapVM vm =(MapVM) e.NewValue;
            Map map = new Map();
            map.SnapToLevels = true;
            map.IsLogoVisible = false;
            Binding bind = new Binding("LayerCollection");
            bind.Source = vm;
            BindingOperations.SetBinding(map, Map.LayersProperty, bind);
            mv.MapNavigator.Map = map;
            mv.grid_Content.Children.Add(map);
           
        }

    }
}
