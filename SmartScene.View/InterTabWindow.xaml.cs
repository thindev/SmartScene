using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartScene.ViewModel;
using SmartScene.ViewModel.Tab;

namespace SmartScene.View
{
    /// <summary>
    /// InterTabWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InterTabWindow : DragablzWindow
    {
        public InterTabWindow(TabWindowVM value)
        {
            InitializeComponent();
            _tabWindowVM = value;
            this.DataContext = _tabWindowVM;
        }
        public TabablzControl TabablzControl
        {
            get { return tc; }
        }

        MainWindowVM _mainWindow;
        public MainWindowVM MainWindowVM
        {
            get
            {
                return _mainWindow;
            }

            set
            {
                _mainWindow = value;
            }
        }


        public TabWindowVM TabWindowVM
        {
            get
            {
                return _tabWindowVM;
            }
        }

        TabWindowVM _tabWindowVM;

        private void DragablzWindow_Closed(object sender, EventArgs e)
        {
            this.MainWindowVM.TabPanelManagerVM.TabWindowVMs.Remove(this.TabWindowVM);
        }
    }
}
