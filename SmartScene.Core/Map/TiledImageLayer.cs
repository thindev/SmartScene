using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;

namespace SmartScene.Core.Map
{
    [Serializable]
    public class TiledImageLayer:BusinessBase<TiledImageLayer>,ILayer
    {
        public static readonly PropertyInfo<TiledImageLayerType> TypeProperty = RegisterProperty<TiledImageLayerType>(b=>b.ServerType);

        public TiledImageLayerType ServerType
        {
            get {return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        public static readonly PropertyInfo<string> UrlProperty = RegisterProperty<string>(b => b.Url);
        public String Url
        {
            get { return GetProperty(UrlProperty); }
            set { SetProperty(UrlProperty,value); }
        }

        public int Index
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string ID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsVisible
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
