using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Dragablz;
using System.Windows.Data;
using SmartScene.ViewModel;
using SmartScene.ViewModel.Tab;

namespace SmartScene.View
{
    public class InterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            TabControlVM tcvm = new TabControlVM();
            TabWindowVM twvm = new TabWindowVM() { };
            twvm.TabControlVMs.Add(tcvm);
            InterTabWindow view = new InterTabWindow(twvm);

            Window window = Window.GetWindow(source);
            TabControlVM tvm = (TabControlVM)source.DataContext;
            MainWindowVM mvm = null;
            if(window is MainWindow)
            {
                mvm = (window as MainWindow).MainWindowVM;
                if (tvm.TabPanelVMs.Count == 1)
                {
                    mvm.TabPanelManagerVM.TabControlVMs.Remove(tvm);
                }
            }
            else if(window is InterTabWindow)
            {
                mvm = (window as InterTabWindow).MainWindowVM;
                if (tvm.TabPanelVMs.Count == 1)
                {
                    (window as InterTabWindow).TabWindowVM.TabControlVMs.Remove(tvm);
                }
            }
            view.MainWindowVM =mvm;
            view.MainWindowVM.TabPanelManagerVM.TabWindowVMs.Add(view.TabWindowVM);
           
            NewTabHost<InterTabWindow> th= new  NewTabHost<InterTabWindow>(view, view.TabablzControl) ;
            return th;
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            if (window is MainWindow)
            {
                MainWindow mw = (window as MainWindow);
                if (mw.MainWindowVM.TabPanelManagerVM.TabControlVMs.Count == 1)
                {
                    return TabEmptiedResponse.DoNothing;
                }
                else
                {
                    TabControlVM vm = tabControl.DataContext as TabControlVM;
                    if (vm.TabPanelVMs.Count == 0)
                    {
                        mw.MainWindowVM.TabPanelManagerVM.TabControlVMs.Remove(vm);
                    }
                }
                    
            }
            else if(window is InterTabWindow)
            {
                TabControlVM vm = tabControl.DataContext as TabControlVM;
                if (vm.TabPanelVMs.Count == 0)
                {
                    (window as InterTabWindow).TabWindowVM.TabControlVMs.Remove(vm);
                }
            }
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
