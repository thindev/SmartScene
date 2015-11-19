using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SmartScene.ViewModel.Tab;

namespace SmartScene.ViewModel
{
    public class MainWindowVM:ViewModelBase
    {
        TabPanelManagerVM _tabPanelManagerVM=new TabPanelManagerVM();
        public TabPanelManagerVM TabPanelManagerVM
        {
            get { return _tabPanelManagerVM; }
            set
            { 
                if(_tabPanelManagerVM!=value)
                {
                    _tabPanelManagerVM = value;
                    OnPropertyChanged(() => TabPanelManagerVM);
                }
            }
        }
    }

}
