using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;
using SmartScene.ViewModel;
using System.Windows.Controls;

namespace SmartScene.View
{
    //lifted most of this code from MahApps demo, to help illustrate the Dragablz themese complying with MahApps.

    public class ThemeManagement: ViewModelBase,IThemeManagement
    {
        public List<AccentColorData> AccentColors { get; set; }
        public List<AppThemeData> AppThemes { get; set; }

        private AccentColorData selectedAccentColor;
        public AccentColorData SelectedAccentColor
        {
            get
            {
                return selectedAccentColor;
            }

            set
            {
                if(selectedAccentColor!=value)
                {
                    selectedAccentColor = value;
                    var theme = ThemeManager.DetectAppStyle(Application.Current);
                    var accent = ThemeManager.GetAccent(selectedAccentColor.Name);
                    ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
                    OnPropertyChanged(()=>SelectedAccentColor);
                }
            }
        }

        public double Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                if(_scale!=value)
                {
                    _scale = value;
                    MetroWindowEx.ScaleTransform.ScaleY = MetroWindowEx.ScaleTransform.ScaleX = value;
                }
            }
        }

        public bool IsMainWindowTitleBarVisible
        {
            get
            {
                return _isMainWindowTitleBarVisible;
            }

            set
            {
                if(_isMainWindowTitleBarVisible!=value)
                {
                    _isMainWindowTitleBarVisible = value;
                    OnPropertyChanged(()=> IsMainWindowTitleBarVisible);
                }
                
            }
        }

        public bool IgnoreTaskbarOnMaximize
        {
            get
            {
                return _ignoreTaskbarOnMaximize;
            }

            set
            {
                if(_ignoreTaskbarOnMaximize!=value)
                {
                    _ignoreTaskbarOnMaximize = value;
                    OnPropertyChanged(()=> IgnoreTaskbarOnMaximize);
                }
                
            }
        }

        double _scale=1.0;

        bool _isMainWindowTitleBarVisible = true;
        bool _ignoreTaskbarOnMaximize = false;

        public ThemeManagement()
        {
            // create accent color menu items for the demo
            this.AccentColors = ThemeManager.Accents
                .Select(
                    a =>
                        new AccentColorData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                .Select(
                    a =>
                        new AppThemeData()
                        {
                            Name = a.Name,
                            BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                            ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                        })
                .ToList();
              Accent ac=  ThemeManager.DetectAppStyle(Application.Current).Item2;
            this.SelectedAccentColor = this.AccentColors.FirstOrDefault(x => x.Name == ac.Name);
        }
    }

    public class AccentColorData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private ICommand changeAccentCommand;

        public ICommand ChangeAccentCommand
        {
            get { return this.changeAccentCommand ?? (changeAccentCommand = new SimpleCommand { CanExecuteDelegate = x => true, ExecuteDelegate = x => this.DoChangeTheme(x) }); }
        }

        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }

    public class AppThemeData : AccentColorData
    {
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
        }
    }

    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true; // if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }
    }
}
