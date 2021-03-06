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

namespace WPF_TestDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int result = Convert.ToInt32(text1.Text.Trim()) + Convert.ToInt32(text2.Text.Trim());
            MessageBox.Show(result.ToString());
            for (int i = 0; i < 360; i++)
            {
                btn1.RenderTransform.Transform(new Point (i,i-10));
            }
        }
    }
}
