using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;

namespace SmartScene.Core.Map
{
    public class TiledImageLayer:BusinessBase<TiledImageLayer>
    {
        public static readonly PropertyInfo<TiledImageLayerType> TypeProperty = RegisterProperty<TiledImageLayerType>(b=>b.Type);

        public TiledImageLayerType Type
        {

        }
    }
}
