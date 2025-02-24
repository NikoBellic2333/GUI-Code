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
    /// SliderCard.xaml 的交互逻辑
    /// </summary>
    public partial class SliderCard : Window
    {
       
        public SliderCard()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index=(int)Slider.Value;
            switch (index)
            {
                case 0:
                    grid0.Visibility = Visibility.Visible;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    grid5.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Visible;
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    grid5.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Visible;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    grid5.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Visible;
                    grid4.Visibility = Visibility.Collapsed;
                    grid5.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    grid5.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
