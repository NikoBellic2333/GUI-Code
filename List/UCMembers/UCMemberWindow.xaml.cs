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
    /// UCMemberWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCMemberWindow : Window
    {
        private ScrollViewer _scrollViewer;
        public UCMemberWindow()
        {
            InitializeComponent();
            Loaded += UCMemberWindow_Loaded;
        }

        private void UCMemberWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取 ListView 内部的 ScrollViewer
            _scrollViewer = GetScrollViewer(listview);

            if (_scrollViewer != null)
            {
                // 设置 Slider 的最大值为 ScrollViewer 的滚动范围
                mySlide.Maximum = _scrollViewer.ScrollableHeight;
                mySlide.Minimum = 0;

                // 监听 ListView 滚动条的滚动同步 Slider 的值
                _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        private void mySlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_scrollViewer != null)
            {
                // 使用 Slider 的值设置 ScrollViewer 的垂直偏移
                _scrollViewer.ScrollToVerticalOffset(mySlide.Value);
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_scrollViewer != null)
            {
                // 同步 Slider 的值为当前滚动偏移
                mySlide.Value = _scrollViewer.VerticalOffset;
            }
        }

        // Helper 方法：递归查找 ScrollViewer
        private ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer viewer)
                return viewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var result = GetScrollViewer(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
