﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.UCMembers
{
    /// <summary>
    /// UCMembers.xaml 的交互逻辑
    /// </summary>
    public partial class UCMembers : UserControl
    {
        public UCMembers()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string URL { get; set; }
    }
}
