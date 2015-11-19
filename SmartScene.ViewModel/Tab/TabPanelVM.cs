using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SmartScene.ViewModel;

namespace SmartScene.ViewModel.Tab
{
    public class TabPanelVM:ViewModelBase
    {
        string _title;
        TabPanelContentType _contentType;
        object _content;

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if(_title!=value)
                {
                    _title = value;
                    OnPropertyChanged(()=>Title);
                }
                
            }
        }

       

        public TabPanelContentType ContentType
        {
            get
            {
                return _contentType;
            }

            set
            {
                if(_contentType!=value)
                {
                    _contentType = value;
                    OnPropertyChanged(()=>ContentType);
                }
                
            }
        }

        public object Content
        {
            get
            {
                return _content;
            }

            set
            {
                if(_content!=value)
                {
                    _content = value;
                    _contentView = null;
                    OnPropertyChanged(()=>Content);
                    OnPropertyChanged(() => ContentView);
                }
            }
        }

        Visual _contentView;
        public Visual ContentView
        {
            get
            {
                if(_contentView==null)
                 _contentView= ViewModelContext.Current.TabContentViewProvider.GetContentView(this);
                return _contentView;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if(_isSelected!=value)
                {
                    _isSelected = value;
                    OnPropertyChanged(()=>IsSelected);
                }
                
            }
        }

        bool _canClose=true;
        public bool CanClose
        {
            get
            {
                return _canClose;
            }

            set
            {
                if(_canClose!=value)
                {
                    _canClose = value;
                    OnPropertyChanged(()=>CanClose);
                }
               
            }
        }

        bool _isSelected;
    }

}
