using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SmartScene.ViewModel
{
    public class AppFrameworkVM:ViewModelBase
    {
        ObservableCollection<MainTabPanel> _mainTabPanels = new ObservableCollection<MainTabPanel>();

        public ObservableCollection<MainTabPanel> MainTabPanels
        {
            get
            {
                return _mainTabPanels;
            }
        }
    }
}
