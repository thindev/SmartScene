using SmartScene.ViewModel.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScene.ViewModel
{
    public class ViewModelContext:ViewModelBase
    {
       public  ITabContentViewProvider TabContentViewProvider { get; set; }
        IThemeManagement  _themeManagement;
       public  IThemeManagement ThemeManagement
       {
           get
           {
               return _themeManagement;
           }

           set
           {
               if (_themeManagement != value)
               {
                   _themeManagement = value;
                   OnPropertyChanged(() => ThemeManagement);
               }

           }
       }

       static  ViewModelContext  _current = new ViewModelContext();
        public static ViewModelContext Current
       {
           get { return _current; }
       }
    
    }
}
