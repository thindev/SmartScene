using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace SmartScene.Core.Map
{
    [Serializable]
     public class MapExtent
    {
       public double XMin { get; private set; }
       public double XMax { get; private set; }
       public double YMin { get; private set; }
       public double YMax { get; private set; }

        public MapExtent(double xMin,double yMin,double xMax,double yMax)
        {
            XMax = xMax; 
            XMin = xMin;
            YMax = yMax;
            YMin = yMin;
        }
    }

}
