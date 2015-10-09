using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;

namespace SmartScene.Core.Map
{
    [Serializable]
    public class SceneMap:BusinessBase<SceneMap>
    {
        public static readonly PropertyInfo<MapExtent> FullExtentProperty = RegisterProperty<MapExtent>(b => b.FullExtent);
        public MapExtent FullExtent
        {
            get { return GetProperty(FullExtentProperty); }
            set { SetProperty(FullExtentProperty, value); }
        }

        public TiledImageLayerCollection TiledImageLayers
        {
            get { return GetProperty(TiledImageLayersProperty); }
            set { SetProperty(TiledImageLayersProperty, value); }
        }

        public static readonly PropertyInfo<TiledImageLayerCollection> TiledImageLayersProperty = RegisterProperty<TiledImageLayerCollection>(b => b.TiledImageLayers);


    }
}
