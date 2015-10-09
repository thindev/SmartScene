using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla.Core;
using SmartScene.Core.Map;

namespace SmartScene.Core
{
    public interface IScene:IEditableBusinessObject
    {
        string Id { get; set; }
        string Name { get; set; }
        bool IsSceneFrame { get; }
        SceneCollection SubScenes { get; set; }
        SceneMap Map { get; set; }
    }
}
