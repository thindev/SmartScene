using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SmartScene.ViewModel;
using SmartScene.ViewModel.Tab;

namespace SmartScene.View
{
    public class TabContentViewProvider : ITabContentViewProvider
    {
        public Visual GetContentView(TabPanelVM tabPanel)
        {
            if(tabPanel.ContentType== TabPanelContentType.Scene)
            {
                SceneView sv= new SceneView();
                sv.DataContext = tabPanel.Content;
                return sv;
            }

            return null;
        }
    }
}
