using Dragablz;
using System;
using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SmartScene.ViewModel;

namespace SmartScene.View
{
    /// <summary>
    /// Provides a simple implementation of <see cref="IInterLayoutClient"/>, but only really useful if 
    /// <see cref="TabItem"/> instances are specified in XAML.  If you are binding via ItemsSource then
    /// you most likely want to create your own implementation of <see cref="IInterLayoutClient"/>.
    /// </summary>
    public class InterLayoutClient : IInterLayoutClient
    {
        public INewTabHost<UIElement> GetNewHost(object partition, TabablzControl source)
        {
            TabControlVM vm = new TabControlVM();
            InterTabWindow mw=(InterTabWindow) Window.GetWindow(source);
            mw.MainWindowVM.TabControlVMs.Add(vm);
            var tabablzControl = new TabablzControl {DataContext = vm};
            tabablzControl.ItemsSource = vm.TabPanelVMs;

            Clone(source, tabablzControl);

            var newInterTabController = new InterTabController
            {
                Partition = source.InterTabController.Partition
            };
            Clone(source.InterTabController, newInterTabController);
            tabablzControl.SetCurrentValue(TabablzControl.InterTabControllerProperty, newInterTabController);            

            return new NewTabHost<UIElement>(tabablzControl, tabablzControl);
        }

        private static void Clone(DependencyObject from, DependencyObject to)
        {
            var localValueEnumerator = from.GetLocalValueEnumerator();
            while (localValueEnumerator.MoveNext())
            {
                if (localValueEnumerator.Current.Property.ReadOnly ||
                    localValueEnumerator.Current.Value is FrameworkElement) continue;
                
                if (!(localValueEnumerator.Current.Value is BindingExpressionBase))
                    to.SetCurrentValue(localValueEnumerator.Current.Property, localValueEnumerator.Current.Value);                
            }            
        }
    }
}