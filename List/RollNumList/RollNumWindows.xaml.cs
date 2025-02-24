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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// RollNumWindows.xaml 的交互逻辑
    /// </summary>
    public partial class RollNumWindows : Window
    {
        public RollNumWindows()
        {
            InitializeComponent();
            TestRollNum.ControlHeight = 300;

            List<string> ls = new List<string>();
            ls.Add("00");
            ls.Add("01");
            ls.Add("02");
            ls.Add("03");
            ls.Add("04");
            ls.Add("05");
            ls.Add("06");
            ls.Add("07");
            ls.Add("08");
            ls.Add("09");
            ls.Add("10");
            ls.Add("11");
            ls.Add("12");
            ls.Add("13");
            ls.Add("14");
            ls.Add("15");
            ls.Add("16");
            ls.Add("17");
            ls.Add("18");
            ls.Add("19");
            ls.Add("20");
            ls.Add("21");
            ls.Add("22");
            ls.Add("23");
            ls.Add("24");
            ls.Add("25");
            ls.Add("26");
            ls.Add("27");
            ls.Add("28");
            ls.Add("29");
            ls.Add("30");
            ls.Add("31");
            TestRollNum.Items = ls;
        }

        private void mySlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TestRollNum.CurrentIndex = (int)mySlide.Value;
        }

    }
}
