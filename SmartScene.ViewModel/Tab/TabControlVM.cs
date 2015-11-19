using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScene.ViewModel.Tab
{
    public class TabControlVM:ViewModelBase
    {

        ObservableCollection<TabPanelVM> _tabPanels = new ObservableCollection<TabPanelVM>();
        public ObservableCollection<TabPanelVM> TabPanelVMs
        {
            get
            {
                return _tabPanels;
            }
        }

        public TabPanelVM SelectedTabPanelVM
        {
            get
            {
                return _selectedTabPanelVM;
            }

            set
            {
                if(_selectedTabPanelVM!=value)
                {
                    _selectedTabPanelVM = value;
                    OnPropertyChanged(()=>SelectedTabPanelVM);
                }
                
            }
        }

        TabPanelVM _selectedTabPanelVM;
    }
}
