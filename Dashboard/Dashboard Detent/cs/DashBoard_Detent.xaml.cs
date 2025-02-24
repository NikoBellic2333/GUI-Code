using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.DashBoard_Detent
{
    /// <summary>
    /// DashBoard_Detent.xaml 的交互逻辑
    /// </summary>
    public partial class DashBoard_Detent : UserControl
    {
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(int), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMinPropertyChanged)));
        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }
        private static void OnMinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.Min = (int)e.NewValue;
            f.RenderGauge();
        }

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(int), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMaxPropertyChanged)));
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        private static void OnMaxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.Max = (int)e.NewValue;
            f.RenderGauge();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.Value = (int)e.NewValue;
            f.RenderGauge();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnTitlePropertyChanged)));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.Title = (string)e.NewValue;
        }

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnUnitPropertyChanged)));
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        private static void OnUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.Unit = (string)e.NewValue;
        }

        public static readonly DependencyProperty TickUnitProperty = DependencyProperty.Register("TickUnit", typeof(string), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnTickUnitPropertyChanged)));
        public string TickUnit
        {
            get { return (string)GetValue(TickUnitProperty); }
            set { SetValue(TickUnitProperty, value); }
        }
        private static void OnTickUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.TickUnit = (string)e.NewValue;
        }

        public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register("ValueString", typeof(int), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueStringPropertyChanged)));
        public int ValueString
        {
            get { return (int)GetValue(ValueStringProperty); }
            set { SetValue(ValueStringProperty, value); }
        }
        private static void OnValueStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.model.ValueString = (int)e.NewValue;
        }


        public static readonly DependencyProperty TickGapProperty = DependencyProperty.Register("TickGap", typeof(int), typeof(DashBoard_Detent), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnTickGapPropertyChanged)));
        public int TickGap
        {
            get { return (int)GetValue(TickGapProperty); }
            set { SetValue(TickGapProperty, value); }
        }
        private static void OnTickGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DashBoard_Detent f = d as DashBoard_Detent;
            f.RenderTicks();
        }

        private const double FULLRANGE = 270;
        private const double STARTANGLE = 90;
        private const double STARTNIDDLEANGLE = 225;
        private const double START_TICK_ANGLE = 135;

        public delegate int RealValueDelegate(int Value);
        public RealValueDelegate GetRealValue { get; set; }
        private Style tickStyle;
        private GaugeModel_Detent model;
        public DashBoard_Detent()
        {
            InitializeComponent();
            tickStyle = this.Resources["TickStyle"] as Style;
            model = this.Resources["model"] as GaugeModel_Detent;
        }


        private void RenderGauge()
        {
            if (GetRealValue == null) return;
            //draw gauge
            int value = Value >= Max ? Max : Value <= Min ? Min : Value;
            int offset = 190;
            double angle = FULLRANGE / (Max - Min) * (Value - Min);
            double rad = (Math.PI / 180) * (angle + STARTANGLE);

            double x = offset * Math.Cos(rad) + offset;
            double y = offset * Math.Sin(rad) + offset;

            Point point = new Point(x, y);
            arcWpf.IsLargeArc = angle > 180;
            arcWpf.Point = point;


            //rotate animation
            DoubleAnimation ani_guage = new DoubleAnimation();
            Duration dur = new Duration(TimeSpan.FromMilliseconds(100));
            ExponentialEase acc = new ExponentialEase();
            acc.EasingMode = EasingMode.EaseOut;
            acc.Exponent = 5;
            ani_guage.To = angle + STARTNIDDLEANGLE;
            ani_guage.Duration = dur;
            ani_guage.EasingFunction = acc;
            Storyboard.SetTargetName(ani_guage, "niddle");
            Storyboard.SetTargetProperty(ani_guage, new PropertyPath(RotateTransform.AngleProperty));

            //print value string
            Int32Animation ani_value_str = new Int32Animation();
            ani_value_str.To = GetRealValue(value);
            ani_value_str.Duration = dur;
            ani_value_str.EasingFunction = acc;
            Storyboard.SetTarget(ani_value_str, this);
            Storyboard.SetTargetProperty(ani_value_str, new PropertyPath(DashBoard_Detent.ValueStringProperty));

            Storyboard sb = new Storyboard();
            sb.Children.Add(ani_guage);
            sb.Children.Add(ani_value_str);
            sb.Begin(this);
        }

        private void RenderTicks()
        {
            if ((Max - Min) < TickGap) return;
            tick_container.Children.Clear();


            int ticks = (Max - Min) / TickGap + 1;
            TextBlock[] textblock = new TextBlock[ticks];

            for (int i = 0; i < ticks; i++)
            {
                textblock[i] = new TextBlock();
                textblock[i].Text = string.Format("{0:F0}", Min + (Max - Min) / (ticks - 1) * i);
                setTickDefaultStyle(textblock[i]);
            }

            double tick_angle_offset = FULLRANGE / (textblock.Length - 1);
            int tick_start_angle = 100;
            int tick_dest_angle = 160;

            ExponentialEase acc = new ExponentialEase();
            acc.EasingMode = EasingMode.EaseInOut;
            acc.Exponent = 1;
            Duration dur = new Duration(TimeSpan.FromMilliseconds(100));
            List<string> names = new List<string>();
            int delay = 100;
            Storyboard sb = new Storyboard();
            for (int i = 0; i < textblock.Length; i++)
            {
                double angle = i * tick_angle_offset + START_TICK_ANGLE;
                double rad = (Math.PI / 180) * angle;
                double x = tick_dest_angle * Math.Cos(rad);
                double y = tick_dest_angle * Math.Sin(rad);

                double start_x = tick_start_angle * Math.Cos(rad);
                double start_y = tick_start_angle * Math.Sin(rad);

                //Set start location of each text block
                TranslateTransform tsl = new TranslateTransform(start_x, start_y);
                textblock[i].RenderTransform = tsl;
                string tsl_name = "translate" + tsl.GetHashCode();
                RegisterName(tsl_name, tsl);
                names.Add(tsl_name);
                tick_container.Children.Add(textblock[i]);

                //move to dest of tick using animation
                DoubleAnimation trans_tick_x = new DoubleAnimation();
                trans_tick_x.To = x;
                trans_tick_x.BeginTime = TimeSpan.FromMilliseconds(i * delay);
                trans_tick_x.Duration = dur;
                trans_tick_x.EasingFunction = acc;
                Storyboard.SetTargetName(trans_tick_x, tsl_name);
                Storyboard.SetTargetProperty(trans_tick_x, new PropertyPath(TranslateTransform.XProperty));


                DoubleAnimation trans_tick_y = new DoubleAnimation();
                trans_tick_y.To = y;
                trans_tick_y.BeginTime = TimeSpan.FromMilliseconds(i * delay);
                trans_tick_y.Duration = dur;
                trans_tick_y.EasingFunction = acc;
                Storyboard.SetTargetName(trans_tick_y, tsl_name);
                Storyboard.SetTargetProperty(trans_tick_y, new PropertyPath(TranslateTransform.YProperty));

                sb.Children.Add(trans_tick_x);
                sb.Children.Add(trans_tick_y);
            }

            // 创建并启动 Storyboard
            sb.Begin(this);

            // 订阅 Storyboard 的 Completed 事件
            sb.Completed += (sender, e) =>
            {
                // 遍历 names 列表中的每个名称
                foreach (string s in names)
                {
                    // 注销名称
                    UnregisterName(s);
                }
            };
        }

        private void setTickDefaultStyle(TextBlock tb)
        {
            if (tb == null) return;
            tb.Style = tickStyle;

        }
    }
}
