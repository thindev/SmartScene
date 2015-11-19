using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartScene.ViewModel.Map;
using System.Collections.ObjectModel;


namespace SmartScene.ViewModel
{
    public class SceneVM:ViewModelBase
    {
        ObservableCollection<MapVM> _mapVMs = new ObservableCollection<MapVM>();
        public ObservableCollection<MapVM> MapVMs
        {
            get
            {
                return _mapVMs;
            }

        }

        MapVM _selectedMapVM;
        public MapVM SelectedMapVM
        {
            get
            {
                return _selectedMapVM;
            }

            set
            {
                if (_selectedMapVM != value)
                {
                    _selectedMapVM = value;
                    OnPropertyChanged(() => SelectedMapVM);
                }
            }
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if(_name!=value)
                {
                    _name = value;
                    OnPropertyChanged(()=>Name);
                }
                
            }
        }
        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                if(_id!=value)
                {
                    _id = value;
                    OnPropertyChanged(()=>Id);
                }
            }
        }

        public string ParentId
        {
            get
            {
                return _parentId;
            }

            set
            {
                if(_parentId!=value)
                _parentId = value;
            }
        }

        public ObservableCollection<SceneVM> SubScenes
        {
            get
            {
                return _subScenes;
            }

         
        }

       

        string _id;

        string _parentId;

        ObservableCollection<SceneVM> _subScenes = new ObservableCollection<SceneVM>();
    }
}
