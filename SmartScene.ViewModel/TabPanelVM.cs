using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScene.ViewModel
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
                    OnPropertyChanged(()=>Content);
                }
                
                
            }
        }
    }

}
