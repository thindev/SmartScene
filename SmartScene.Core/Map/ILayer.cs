using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScene.Core.Map
{
    public interface ILayer:Csla.Core.IEditableBusinessObject
    {
        string ID { get; set; }
        int Index { get; set; }
        bool IsVisible { get; set; }
        string Name { get; set; }
    }
}
