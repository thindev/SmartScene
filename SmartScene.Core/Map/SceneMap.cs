using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;

namespace SmartScene.Core.Map
{
    public class SceneMap:BusinessBase<SceneMap>
    {
        public static readonly PropertyInfo<MapExtent> FullExtentProperty = RegisterProperty<MapExtent>(b => b.FullExtent);
        public MapExtent FullExtent
        {
            get { return GetProperty(FullExtentProperty); }
            set { SetProperty(FullExtentProperty, value); }
        }

        public TiledImageLayer TiledImageLayer
        {
            get { return GetProperty(TiledImageLayerProperty); }
            set { SetProperty(TiledImageLayerProperty, value); }
        }

        public static readonly PropertyInfo<TiledImageLayer> TiledImageLayerProperty = RegisterProperty<TiledImageLayer>(b => b.TiledImageLayer);
    }
}
