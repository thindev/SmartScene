using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartScene.Core;


namespace SmartScene.ViewModel
{
    public class SceneVM:ViewModelBase
    {
        private Scene _madel;
        public Scene Model
        {
            get { return _madel; }
        }
        public SceneVM(Scene model)
        {
            _madel = model;
        }
    }
}
