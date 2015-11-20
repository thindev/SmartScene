using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using ESRI.ArcGIS.Client.Symbols;
using System.Collections.Generic;

namespace SmartScene.View
{

    [TemplatePart(Name = "DrawForZoomIn", Type = typeof(ToggleButton)), TemplatePart(Name = "DrawForZoomOut", Type = typeof(ToggleButton)), TemplatePart(Name = "DragPan", Type = typeof(ToggleButton)), TemplatePart(Name = "PreviousExtent", Type = typeof(Button)), TemplatePart(Name = "NextExtent",Type=typeof(Button))]
    [TemplatePart(Name = "TransformRotate", Type = typeof(RotateTransform)), TemplatePart(Name = "RotateRing", Type = typeof(FrameworkElement)), TemplatePart(Name = "PanLeft", Type = typeof(RepeatButton)), TemplatePart(Name = "PanRight", Type = typeof(RepeatButton)), TemplatePart(Name = "PanUp", Type = typeof(RepeatButton)), TemplatePart(Name = "PanDown", Type = typeof(RepeatButton)), TemplatePart(Name = "ZoomSlider", Type = typeof(Slider)), TemplatePart(Name = "ZoomOutButton", Type = typeof(Button)), TemplatePart(Name = "ZoomFullExtent", Type = typeof(Button)), TemplatePart(Name = "ResetRotation", Type = typeof(Button)), TemplatePart(Name = "ZoomInButton", Type = typeof(Button))]
    public class MapNavigator:Control
    {
        private double _panFactor = 0.5;
        private double angle;
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(ESRI.ArcGIS.Client.Map), typeof(MapNavigator), new PropertyMetadata(new PropertyChangedCallback(MapNavigator.OnMapPropertyChanged)));
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(MapNavigator), new FrameworkPropertyMetadata(300), new ValidateValueCallback(IsDelayValid));
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(MapNavigator), new FrameworkPropertyMetadata(100), new ValidateValueCallback(IsIntervalValid));
        private bool mouseOver;

        private ToggleButton DrawForZoomIn;
        private ToggleButton DrawForZoomOut;
        private ToggleButton DragPan;
        private Button PreviousExtent;
        private Button NextExtent;

        private RepeatButton PanDown;
        private RepeatButton PanLeft;
        private RepeatButton PanRight;
        private RepeatButton PanUp;
        private Button ResetRotation;
        private FrameworkElement RotateRing;
        private Point startMousePos;
        private bool trackingRotation;
        private RotateTransform TransformRotate;
        private Button ZoomFullExtent;
        private Button ZoomInButton;
        private Button ZoomOutButton;
        private Slider ZoomSlider;

        //**********************************
        private SimpleFillSymbol defaultFillSymbol = new SimpleFillSymbol() { BorderBrush=Brushes.Red };
        private Draw myDrawObject;
        private List<ToggleButton> toggleButtons = new List<ToggleButton>();
        private ToggleButton checkedToggleButton;
        List<Envelope> _extentHistory = new List<Envelope>();
        int _currentExtentIndex = 0;
        bool _newExtent = true;

        public bool AutoUncheckZoomButton { get; set; }

        public MapNavigator()
        {
            this.AutoUncheckZoomButton = true;
        }
        private void InitDrawObject()
        {
            if(myDrawObject==null)
            {
                myDrawObject = new Draw();
                myDrawObject.DrawComplete += myDrawObject_DrawComplete;
                myDrawObject.FillSymbol = this.defaultFillSymbol;
                myDrawObject.DrawMode = DrawMode.Rectangle;
            }
            myDrawObject.Map = this.Map;
        }

        private void ResetExtentHistory()
        {
            this._extentHistory.Clear();
            this._currentExtentIndex = 0;
            this._newExtent = true;
            if (this.NextExtent != null)
            {
                this.NextExtent.Opacity = 0.3;
                this.NextExtent.IsHitTestVisible = false;
            }
            if(this.PreviousExtent!=null)
            {
                this.PreviousExtent.Opacity = 0.3;
                this.PreviousExtent.IsHitTestVisible = false;
            }
        }

        private void EnableTogglebutton(ToggleButton tgb)
        {
            if (tgb == null) return;
            if(!this.toggleButtons.Contains(tgb))
            {
                tgb.Checked += tgb_Checked;
                tgb.Unchecked += Tgb_Unchecked;
                toggleButtons.Add(tgb);
            }
        }

        private void Tgb_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender == this.DrawForZoomIn || sender == this.DrawForZoomOut)
            {
                myDrawObject.IsEnabled = false;
                isDrawComplete = true;
            }
        }

        void tgb_Checked(object sender, RoutedEventArgs e)
        {
            this.checkedToggleButton = sender as ToggleButton;
           foreach(ToggleButton tg in this.toggleButtons)
           {
               if(tg!=sender)
               {
                   tg.IsChecked = false;
               }
               if(sender==this.DrawForZoomIn||sender==this.DrawForZoomOut)
               {
                   myDrawObject.IsEnabled = true;
                    isDrawComplete = false;
                }
               else
               {
                   myDrawObject.IsEnabled = false;
               }
           }
        }

        bool isDrawComplete;
        private void myDrawObject_DrawComplete(object sender, DrawEventArgs args)
        {
            

            if (this.checkedToggleButton == this.DrawForZoomIn)
            {
                this.Map.ZoomTo(args.Geometry as ESRI.ArcGIS.Client.Geometry.Envelope);
            }
            else if (this.checkedToggleButton == this.DrawForZoomOut)
            {
                Envelope currentExtent = this.Map.Extent;
                Envelope zoomBoxExtent = args.Geometry as Envelope;
                MapPoint zoomBoxCenter = zoomBoxExtent.GetCenter();

                double whRatioCurrent = currentExtent.Width / currentExtent.Height;
                double whRatioZoomBox = zoomBoxExtent.Width / zoomBoxExtent.Height;

                Envelope newEnv = null;

                if (whRatioZoomBox > whRatioCurrent)
                // use width
                {
                    double mapWidthPixels = this.Map.Width;
                    double multiplier = currentExtent.Width / zoomBoxExtent.Width;
                    double newWidthMapUnits = currentExtent.Width * multiplier;
                    newEnv = new Envelope(new MapPoint(zoomBoxCenter.X - (newWidthMapUnits / 2), zoomBoxCenter.Y),
                                                   new MapPoint(zoomBoxCenter.X + (newWidthMapUnits / 2), zoomBoxCenter.Y));
                }
                else
                // use height
                {
                    double mapHeightPixels = this.Map.Height;
                    double multiplier = currentExtent.Height / zoomBoxExtent.Height;
                    double newHeightMapUnits = currentExtent.Height * multiplier;
                    newEnv = new Envelope(new MapPoint(zoomBoxCenter.X, zoomBoxCenter.Y - (newHeightMapUnits / 2)),
                                                   new MapPoint(zoomBoxCenter.X, zoomBoxCenter.Y + (newHeightMapUnits / 2)));
                }

                if (newEnv != null)
                {
                    this.Map.ZoomTo(newEnv);
                }
                  
            }
            isDrawComplete = true;

        }

        void NextExtent_Click(object sender, RoutedEventArgs e)
        {
            if (_currentExtentIndex < _extentHistory.Count - 1)
            {
                _currentExtentIndex++;

                if ((this.NextExtent!=null)&&(_currentExtentIndex == (_extentHistory.Count - 1)))
                {
                    this.NextExtent.Opacity = 0.3;
                    this.NextExtent.IsHitTestVisible = false;
                }

                _newExtent = false;

                this.Map.IsHitTestVisible = false;
                this.Map.ZoomTo(_extentHistory[_currentExtentIndex]);

                if ((this.PreviousExtent != null) && (this.PreviousExtent.IsHitTestVisible == false))
                {
                    this.PreviousExtent.Opacity = 1;
                    this.PreviousExtent.IsHitTestVisible = true;
                }
            }
        }

        void PreviousExtent_Click(object sender, RoutedEventArgs e)
        {
            if (_currentExtentIndex != 0)
            {
                _currentExtentIndex--;

                if ((this.PreviousExtent != null) && (_currentExtentIndex == 0))
                {
                    this.PreviousExtent.Opacity = 0.3;
                    this.PreviousExtent.IsHitTestVisible = false;
                }

                _newExtent = false;

                this.Map.IsHitTestVisible = false;
                this.Map.ZoomTo(_extentHistory[_currentExtentIndex]);

                if ((this.NextExtent!=null)&&(this.NextExtent.IsHitTestVisible == false))
                {
                    this.NextExtent.Opacity = 1;
                    this.NextExtent.IsHitTestVisible = true;
                }
            }
        }
    
    
        


        //**********************************






        static MapNavigator()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MapNavigator), new FrameworkPropertyMetadata(typeof(MapNavigator)));
        }

       

        private static bool IsDelayValid(object value)
        {
            return (((int)value) >= 0);
        }

        private static bool IsIntervalValid(object value)
        {
            return (((int)value) > 0);
        }

        public int Delay
        {
            get
            {
                return (int)base.GetValue(DelayProperty);
            }
            set
            {
                base.SetValue(DelayProperty, value);
            }
        }

        public int Interval
        {
            get
            {
                return (int)base.GetValue(IntervalProperty);
            }
            set
            {
                base.SetValue(IntervalProperty, value);
            }
        }
        
        private void ChangeVisualState(bool useTransitions)
        {
            if (this.mouseOver)
            {
                this.GoToState(useTransitions, "MouseOver");
            }
            else
            {
                this.GoToState(useTransitions, "Normal");
            }
        }

        private void enablePanElement(RepeatButton element)
        {
            if (element != null)
            {
                element.Click += panElement_Click;
               // element.MouseLeave += new System.Windows.Input.MouseEventHandler(this.panElement_MouseLeftButtonUp);
              //  element.AddHandler(Button.MouseLeaveEvent, new MouseEventHandler(this.panElement_MouseLeftButtonUp), true);
             //   element.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.panElement_MouseLeftButtonDown);
              //  element.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.panElement_MouseLeftButtonDown), true);
               // element.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.panElement_MouseLeftButtonUp);
             //   element.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.panElement_MouseLeftButtonUp), true);
            }
        }

        
        private double GetAngle(Point a, Point b)
        {
            return ((Math.Atan2(b.X - a.X, a.Y - b.Y) / 3.1415926535897931) * 180.0);
        }
        
        private bool GoToState(bool useTransitions, string stateName)
        {
            return VisualStateManager.GoToState(this, stateName, useTransitions);
        }
        
        private void Layers_LayersInitialized(object sender, EventArgs args)
        {
            this.SetupZoom();
        }

        private void Map_ExtentChanging(object sender, ExtentEventArgs args)
        {
            if (!double.IsNaN(this.Map.Resolution) && (this.ZoomSlider != null))
            {
                this.ZoomSlider.Value = this.ResolutionToValue(this.Map.Resolution);
            }
        }

        private void Map_ExtentChanged(object sender, ExtentEventArgs args)
        {
            if (!double.IsNaN(this.Map.Resolution) && (this.ZoomSlider != null))
            {
                this.ZoomSlider.Value = this.ResolutionToValue(this.Map.Resolution);
            }
            if (args.OldExtent == null)
            {
                _extentHistory.Add(args.NewExtent.Clone());
                return;
            }

            if (_newExtent)
            {
                _currentExtentIndex++;

                if (_extentHistory.Count - _currentExtentIndex > 0)
                    _extentHistory.RemoveRange(_currentExtentIndex, (_extentHistory.Count - _currentExtentIndex));

                if ((this.NextExtent!=null) && (this.NextExtent.IsHitTestVisible == true))
                {
                    this.NextExtent.Opacity = 0.3;
                    this.NextExtent.IsHitTestVisible = false;
                }

                _extentHistory.Add(args.NewExtent.Clone());

                if ((this.PreviousExtent!=null)&&(this.PreviousExtent.IsHitTestVisible == false))
                {
                    this.PreviousExtent.Opacity = 1;
                    this.PreviousExtent.IsHitTestVisible = true;
                }
            }
            else
            {
                this.Map.IsHitTestVisible = true;
                _newExtent = true;
            }
          
            if (this.checkedToggleButton!=null&&this.AutoUncheckZoomButton&&this.isDrawComplete)
            {
                this.checkedToggleButton.IsChecked = false;
            }
        }
        
        private void Map_RotationChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            double num = (base.FlowDirection == FlowDirection.LeftToRight) ? ((double) e.NewValue) : -((double) e.NewValue);
            if ((this.TransformRotate != null) && (this.TransformRotate.Angle != num))
            {
                this.TransformRotate.Angle = num;
            }
            this.angle = (double) e.NewValue;
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.RotateRing = base.GetTemplateChild("RotateRing") as FrameworkElement;
            this.TransformRotate = base.GetTemplateChild("TransformRotate") as RotateTransform;
            if ((this.TransformRotate != null) && (this.Map != null))
            {
                this.TransformRotate.Angle = this.Map.Rotation;
            }
            this.DrawForZoomIn = base.GetTemplateChild("DrawForZoomIn") as ToggleButton;
            this.EnableTogglebutton(this.DrawForZoomIn);
            this.DrawForZoomOut = base.GetTemplateChild("DrawForZoomOut") as ToggleButton;
            this.EnableTogglebutton(this.DrawForZoomOut);
            this.DragPan = base.GetTemplateChild("DragPan") as ToggleButton;
            this.EnableTogglebutton(this.DragPan);
            this.PreviousExtent = base.GetTemplateChild("PreviousExtent") as Button;
            this.NextExtent = base.GetTemplateChild("NextExtent") as Button;
            if (this.PreviousExtent != null)
            {
                this.PreviousExtent.Click += PreviousExtent_Click;
                this.PreviousExtent.Opacity = 0.3;
                this.PreviousExtent.IsHitTestVisible = false;
            }
            if (this.NextExtent != null)
            {
                this.NextExtent.Click += NextExtent_Click;
                this.NextExtent.Opacity = 0.3;
                this.NextExtent.IsHitTestVisible = false;
            }



            this.PanLeft = base.GetTemplateChild("PanLeft") as RepeatButton;
            this.PanRight = base.GetTemplateChild("PanRight") as RepeatButton;
            this.PanUp = base.GetTemplateChild("PanUp") as RepeatButton;
            this.PanDown = base.GetTemplateChild("PanDown") as RepeatButton;
            this.ZoomSlider = base.GetTemplateChild("ZoomSlider") as Slider;
            this.ZoomFullExtent = base.GetTemplateChild("ZoomFullExtent") as Button;
            this.ResetRotation = base.GetTemplateChild("ResetRotation") as Button;
            this.ZoomInButton = base.GetTemplateChild("ZoomInButton") as Button;
            this.ZoomOutButton = base.GetTemplateChild("ZoomOutButton") as Button;
            this.enablePanElement(this.PanLeft);
            this.enablePanElement(this.PanRight);
            this.enablePanElement(this.PanUp);
            this.enablePanElement(this.PanDown);
            base.FlowDirection = FlowDirection.LeftToRight;
            if (this.ZoomSlider != null)
            {
                if (this.Map != null)
                {
                    this.SetupZoom();
                }
                this.ZoomSlider.Minimum = 0.0;
                this.ZoomSlider.Maximum = 1.0;
                this.ZoomSlider.SmallChange = 0.01;
                this.ZoomSlider.LargeChange = 0.1;
                this.ZoomSlider.LostMouseCapture += new System.Windows.Input.MouseEventHandler(this.ZoomSlider_LostMouseCapture);
                this.ZoomSlider.LostFocus += new RoutedEventHandler(this.ZoomSlider_LostMouseCapture);
            }
            if (this.ZoomInButton != null)
            {
                this.ZoomInButton.Click += new RoutedEventHandler(this.ZoomInButton_Click);
            }
            if (this.ZoomOutButton != null)
            {
                this.ZoomOutButton.Click += new RoutedEventHandler(this.ZoomOutButton_Click);
            }
            if (this.RotateRing != null)
            {
                this.RotateRing.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.RotateRing_MouseLeftButtonDown);
                this.RotateRing.LostMouseCapture += new System.Windows.Input.MouseEventHandler(this.RotateRing_OnLostCapture);
            }
            if (this.ZoomFullExtent != null)
            {
                this.ZoomFullExtent.Click += new RoutedEventHandler(this.ZoomFullExtent_Click);
            }
            if (this.ResetRotation != null)
            {
                this.ResetRotation.Click += new RoutedEventHandler(this.ResetRotation_Click);
            }
            bool isInDesignMode = DesignerProperties.GetIsInDesignMode(this);
            if (isInDesignMode)
            {
                this.mouseOver = isInDesignMode;
            }
            this.ChangeVisualState(false);
        }

       
        
        private static void OnMapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapNavigator navigation = d as MapNavigator;
            ESRI.ArcGIS.Client.Map newValue = e.NewValue as ESRI.ArcGIS.Client.Map;
            ESRI.ArcGIS.Client.Map oldValue = e.OldValue as ESRI.ArcGIS.Client.Map;
            if (oldValue != null)
            {
                oldValue.RotationChanged -= new ESRI.ArcGIS.Client.Map.RotationChangedEventHandler(navigation.Map_RotationChanged);
                oldValue.ExtentChanged -= new EventHandler<ExtentEventArgs>(navigation.Map_ExtentChanged);
                oldValue.ExtentChanging -= new EventHandler<ExtentEventArgs>(navigation.Map_ExtentChanging);
                if (oldValue.Layers != null)
                {
                    oldValue.Layers.LayersInitialized -= new LayerCollection.LayersInitializedHandler(navigation.Layers_LayersInitialized);
                }
            }
            if (newValue != null)
            {
                newValue.RotationChanged += new ESRI.ArcGIS.Client.Map.RotationChangedEventHandler(navigation.Map_RotationChanged);
                newValue.ExtentChanged += new EventHandler<ExtentEventArgs>(navigation.Map_ExtentChanged);
                newValue.ExtentChanging += new EventHandler<ExtentEventArgs>(navigation.Map_ExtentChanging);
                if (newValue.Layers != null)
                {
                    newValue.Layers.LayersInitialized += new LayerCollection.LayersInitializedHandler(navigation.Layers_LayersInitialized);
                }
                if (navigation.TransformRotate != null)
                {
                    navigation.TransformRotate.Angle = newValue.Rotation;
                }
                navigation.SetupZoom();
                navigation.InitDrawObject();
                navigation.ResetExtentHistory();
            }
        }
        
        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            this.mouseOver = true;
            this.ChangeVisualState(true);
            base.OnMouseEnter(e);
        }
        
        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            this.mouseOver = false;
            if (!this.trackingRotation)
            {
                this.ChangeVisualState(true);
            }
            base.OnMouseLeave(e);
        }
        
        private void panElement_Click(object sender, RoutedEventArgs e)
        {
            if ((this.Map != null) && (sender != null))
            {
                this.RepeatPan(sender);
            }
        }


    
        private void RepeatPan(object sender)
        {
            Envelope extent = this.Map.Extent;
            if (extent != null)
            {
                double x = 0.0;
                double y = 0.0;
                MapPoint center = extent.GetCenter();
                MapPoint geometry = null;
                double num3 = extent.Height * this._panFactor;
                double num4 = extent.Width * this._panFactor;
                if (sender == this.PanUp)
                {
                    y = center.Y + num3;
                    geometry = new MapPoint(center.X, y);
                }
                else if (sender == this.PanRight)
                {
                    x = center.X + num4;
                    geometry = new MapPoint(x, center.Y);
                }
                else if (sender == this.PanLeft)
                {
                    x = center.X - num4;
                    geometry = new MapPoint(x, center.Y);
                }
                else if (sender == this.PanDown)
                {
                    y = center.Y - num3;
                    geometry = new MapPoint(center.X, y);
                }
                if (geometry != null)
                {
                    this.Map.PanTo(geometry);
                }
            }
        }

      
       
        
        private void ResetRotation_Click(object sender, RoutedEventArgs e)
        {
            Storyboard s = new Storyboard {
                Duration = TimeSpan.FromMilliseconds(500.0)
            };
            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();
            SplineDoubleKeyFrame frame2 = new SplineDoubleKeyFrame {
                KeyTime = s.Duration.TimeSpan,
                Value = 0.0
            };
            KeySpline spline = new KeySpline {
                ControlPoint1 = new Point(0.0, 0.1),
                ControlPoint2 = new Point(0.1, 1.0)
            };
            frame2.KeySpline = spline;
            SplineDoubleKeyFrame keyFrame = frame2;
            frames.KeyFrames.Add(keyFrame);
            keyFrame.Value = 0.0;
            frames.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Rotation", new object[0]));
            s.Children.Add(frames);
            Storyboard.SetTarget(frames, this.Map);
            s.Completed += delegate (object sender2, EventArgs e2) {
                s.Stop();
                this.Map.Rotation = 0.0;
            };
            s.Begin();
        }
        
        private double ResolutionToValue(double resolution)
        {
            double num = Math.Log10(this.Map.MaximumResolution);
            double num2 = Math.Log10(this.Map.MinimumResolution);
            double num3 = 1.0 - ((Math.Log10(resolution) - num2) / (num - num2));
            double value= Math.Min(1.0, Math.Max(num3, 0.0));
            if(double.IsNaN(value))
            {
                return 0.1;
            }
            return value;
        }
        
        private void RotateRing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.startAppMouseTracking();
            this.startMousePos = e.GetPosition(this);
        }
        
        private void RotateRing_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.Map != null)
            {
                FrameworkElement parent = sender as FrameworkElement;
                if ((this.TransformRotate != null) && (parent != null))
                {
                    while (parent.RenderTransform != this.TransformRotate)
                    {
                        parent = parent.Parent as FrameworkElement;
                        if (parent == null)
                        {
                            return;
                        }
                    }
                    Point position = e.GetPosition(this);
                    Point renderTransformOrigin = parent.RenderTransformOrigin;
                    renderTransformOrigin = new Point(parent.ActualWidth * renderTransformOrigin.X, parent.ActualHeight * renderTransformOrigin.Y);
                    renderTransformOrigin = parent.TransformToVisual(this).Transform(renderTransformOrigin);
                    double angle = this.GetAngle(renderTransformOrigin, this.startMousePos);
                    double num2 = this.GetAngle(renderTransformOrigin, position);
                    this.angle = this.Map.Rotation + (num2 - angle);
                    this.startMousePos = position;
                    this.SetMapRotation(this.angle);
                }
            }
        }
        
        private void RotateRing_OnLostCapture(object sender, EventArgs e)
        {
            this.stopAppMouseTracking();
        }
        
        private void SetMapRotation(double angle)
        {
            if (this.TransformRotate != null)
            {
                this.TransformRotate.Angle = (base.FlowDirection == FlowDirection.LeftToRight) ? angle : -angle;
            }
            if (this.Map != null)
            {
                this.Map.Rotation = angle;
            }
        }
        
        public void SetupZoom()
        {
            if ((this.ZoomSlider != null) && (this.Map != null))
            {
                if (((!double.IsNaN(this.Map.MinimumResolution) && !double.IsNaN(this.Map.MaximumResolution)) && ((this.Map.MaximumResolution != double.MaxValue) && (this.Map.MinimumResolution != double.Epsilon))) && !double.IsNaN(this.Map.Resolution))
                {
                    this.ZoomSlider.Value = this.ResolutionToValue(this.Map.Resolution);
                    this.ZoomSlider.Visibility = Visibility.Visible;
                }
                else if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    this.ZoomSlider.Visibility = Visibility.Collapsed;
                }
            }
        }
        
        private void startAppMouseTracking()
        {
            this.trackingRotation = true;
            this.RotateRing.MouseMove += new System.Windows.Input.MouseEventHandler(this.RotateRing_MouseMove);
            this.RotateRing.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.RotateRing_OnLostCapture);
            this.RotateRing.CaptureMouse();
        }
        
        private void stopAppMouseTracking()
        {
            this.RotateRing.ReleaseMouseCapture();
            this.RotateRing.MouseLeftButtonUp -= new System.Windows.Input.MouseButtonEventHandler(this.RotateRing_OnLostCapture);
            this.RotateRing.MouseMove -= new System.Windows.Input.MouseEventHandler(this.RotateRing_MouseMove);
            this.trackingRotation = false;
            this.ChangeVisualState(true);
        }
        
        private double ValueToResolution(double value)
        {
            double num = Math.Log10(this.Map.MaximumResolution);
            double num2 = Math.Log10(this.Map.MinimumResolution);
            double y = ((1.0 - value) * (num - num2)) + num2;
            return Math.Pow(10.0, y);
        }
        
        private void ZoomFullExtent_Click(object sender, RoutedEventArgs e)
        {
            if (this.Map != null)
            {
                if(this.Map.Layers.Count>0&&this.Map.Layers[0] is TiledLayer)
                {
                    this.Map.ZoomTo(this.Map.Layers[0].FullExtent);
                }
            }
        }
        
        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            this.Map.Zoom(this.Map.ZoomFactor);
        }
        
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Map.Zoom(1.0 / this.Map.ZoomFactor);
        }
        
        private void ZoomSlider_LostMouseCapture(object sender, EventArgs e)
        {
            this.Map.ZoomToResolution(this.ValueToResolution(this.ZoomSlider.Value));
        }
        
        public ESRI.ArcGIS.Client.Map Map
        {
            get
            {
                return (base.GetValue(MapProperty) as ESRI.ArcGIS.Client.Map);
            }
            set
            {
                base.SetValue(MapProperty, value);
            }
        }
        
        public double PanFactor
        {
            get
            {
                return this._panFactor;
            }
            set
            {
                this._panFactor = value;
            }
        }
    }
}
