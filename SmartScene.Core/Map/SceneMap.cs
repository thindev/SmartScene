using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;
using GeoAPI.Geometries;


namespace SmartScene.Core.Map
{
    [Serializable]
    public class SceneMap:BusinessBase<SceneMap>
    {
        public static readonly PropertyInfo<Envelope> FullExtentProperty = RegisterProperty<Envelope>(b => b.FullExtent);
        public Envelope FullExtent
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
