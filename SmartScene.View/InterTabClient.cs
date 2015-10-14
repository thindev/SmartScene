using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Dragablz;
using System.Windows.Data;

namespace SmartScene.View
{
    public class InterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var view = new InterTabWindow();
            NewTabHost<InterTabWindow> th= new  NewTabHost<InterTabWindow>(view, view.TabablzControl) ;
            return th;
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {

            if (window is MainWindow)
            {
                MainWindow mw = (window as MainWindow);
                if(mw.dockLayout.Content is TabablzControl)
                {
                    return TabEmptiedResponse.DoNothing;
                }
                if (tabControl.ItemsSource != null)
                {
                    System.Collections.ObjectModel.ObservableCollection<SmartScene.ViewModel.MainTabPanel> source = (System.Collections.ObjectModel.ObservableCollection <SmartScene.ViewModel.MainTabPanel>) tabControl.ItemsSource;
                    Dragablz.Dockablz.Branch b = (Dragablz.Dockablz.Branch)mw.dockLayout.Content;
                    if ((b.FirstItem is Dragablz.TabablzControl) && b.FirstItem != tabControl)
                    {
                        TabablzControl tc = b.FirstItem as Dragablz.TabablzControl;
                        Binding bind = new Binding();
                        bind.Source = source; 
                        
                        foreach(var item in tc.Items)
                        {
                            source.Add(item as SmartScene.ViewModel.MainTabPanel);
                        }
                        tc.Items.Clear();
                        BindingOperations.SetBinding(tc, TabablzControl.ItemsSourceProperty, bind);
                    }
                    else if ((b.SecondItem is Dragablz.TabablzControl)&&b.SecondItem!=tabControl)
                    {
                        TabablzControl tc = b.SecondItem as Dragablz.TabablzControl;
                        Binding bind = new Binding();
                        bind.Source = source;
                        foreach (var item in tc.Items)
                        {
                            source.Add(item as SmartScene.ViewModel.MainTabPanel);
                        }
                        tc.Items.Clear();
                        BindingOperations.SetBinding(tc,TabablzControl.ItemsSourceProperty,bind);
                    }
                }
                
            }
           
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
