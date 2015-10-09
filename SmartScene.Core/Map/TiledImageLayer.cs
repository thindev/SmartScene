using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;

namespace SmartScene.Core.Map
{
    [Serializable]
    public class TiledImageLayer:BusinessBase<TiledImageLayer>
    {
        public static readonly PropertyInfo<TiledImageLayerType> TypeProperty = RegisterProperty<TiledImageLayerType>(b=>b.Type);

        public TiledImageLayerType Type
        {
            get {return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }

        public object Url
        {
            get { return GetProperty(UrlProperty); }
            set { SetProperty(UrlProperty,value); }
        }

        public static readonly PropertyInfo<string> UrlProperty = RegisterProperty<string>(b => b.Url);
    }
}
