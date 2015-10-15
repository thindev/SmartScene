using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScene.ViewModel
{
    public class TabControlVM
    {

        ObservableCollection<TabPanelVM> _tabPanels = new ObservableCollection<TabPanelVM>();
        public ObservableCollection<TabPanelVM> TabPanelVMs
        {
            get
            {
                return _tabPanels;
            }
        }
    }
}
