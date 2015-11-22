using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartScene.View
{
    public class MetroWindowEx:MetroWindow
    {
        public static ScaleTransform ScaleTransform = new ScaleTransform();
       
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid grid=(Grid) base.GetVisualChild(0); 
            grid.LayoutTransform = ScaleTransform;
        }
    }
}
