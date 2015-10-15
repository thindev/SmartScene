using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Dragablz;
using System.Windows.Data;
using SmartScene.ViewModel;

namespace SmartScene.View
{
    public class InterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            InterTabWindow view = new InterTabWindow();
            Window window = Window.GetWindow(source);
            TabControlVM tvm = (TabControlVM)source.DataContext;

            MainWindowVM mvm = null;
            if(window is MainWindow)
            {
                mvm = (window as MainWindow).MainWindowVM;
                //if(tvm.TabPanelVMs.Count==1)
                //{
                //    mvm.TabControlVMs.Remove(tvm);
                //}
            }
            else if(window is InterTabWindow)
            {
                mvm = (window as InterTabWindow).MainWindowVM;
            }
            view.MainWindowVM =mvm;
            view.TabWindowVM = new TabWindowVM() { TabControlVM= new TabControlVM() };
            view.MainWindowVM.TabWindowVMs.Add(view.TabWindowVM);
           
            NewTabHost<InterTabWindow> th= new  NewTabHost<InterTabWindow>(view, view.TabablzControl) ;
            return th;
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            if (window is MainWindow)
            {
                MainWindow mw = (window as MainWindow);
                if (mw.MainWindowVM.TabControlVMs.Count > 1)
                    mw.MainWindowVM.TabControlVMs.Remove(tabControl.DataContext as TabControlVM);
                else
                    return TabEmptiedResponse.DoNothing;
            }
           else if(window is InterTabWindow)
            {
                InterTabWindow ittWin = (InterTabWindow)window;
                MainWindowVM mvm = ittWin.MainWindowVM;
                mvm.TabWindowVMs.Remove(ittWin.TabWindowVM);
            }
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
