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
using SmartScene.Core.Map;
using ESRI.ArcGIS.Client;

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
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(SceneMap), typeof(MapView), new PropertyMetadata(null, new PropertyChangedCallback(MapPropertyChangedCallback)));
        public SceneMap Map
        {
            get {return(SceneMap) base.GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }
        private static void MapPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mv = (MapView)d;
            mv.grid_Content.Children.Clear();
            mv._map= mv.GenerateMapControl();
            mv.grid_Content.Children.Add(mv._map);
            SceneMap sMap = e.NewValue as SceneMap;
            if(sMap!=null&&sMap.Layers!=null)
            {
                sMap.Layers.CollectionChanged += mv.Layers_CollectionChanged;
                sMap.Layers.ChildChanged += mv.Layers_ChildChanged;
               ILayer[] layers= sMap.Layers.OrderBy(x => x.Index).ToArray();
               foreach (ILayer layer in layers)
                {
                    if(layer is TiledImageLayer)
                    {
                        TiledImageLayer tiLayer = layer as TiledImageLayer;
                        TiledMapServiceLayer tiledLayer=null;
                        switch (tiLayer.ServerType)
                        {
                           
                            case TiledImageLayerType.ArcgisTiledLayer:
                                {
                                    tiledLayer = new ArcGISTiledMapServiceLayer() { Url = tiLayer.Url };
                                    break;
                                }
                        }
                        tiledLayer.Visible = layer.IsVisible;
                        tiledLayer.ID = layer.ID;
                        if(tiledLayer!=null)
                        {
                            mv._map.Layers.Add(tiledLayer);
                        }
                        
                    }
                }
            }
        }

        private  void Layers_ChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
        {
            
            throw new NotImplementedException();
        }

        private  void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ClearOldMapValue(SceneMap map)
        {
            if (map == null) return;

        }

        private Map GenerateMapControl()
        {
            Map map = new Map();
            map.IsLogoVisible = false;
            return map;
        }
    }
}
