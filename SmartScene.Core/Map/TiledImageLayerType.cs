﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScene.Core.Map
{
    [Serializable]
    public enum TiledImageLayerType
    {
        ArcgisTiledLayer,
        GoogleTiledLayer,
        BaiduTiledLayer,
    }
}
