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
    /// MagicBarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MagicBarWindow : Window
    {
        public MagicBarWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bar.SelectedIndex = 0;
        }
        private void mySlide_ValueChanged(object sender, RoutedEventArgs e)
        {
            bar.SelectedIndex = (int)mySlide.Value;
        }
    }
}
