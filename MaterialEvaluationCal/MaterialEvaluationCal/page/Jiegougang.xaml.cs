﻿using MaterialEvaluationCal.Calculates;
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

namespace MaterialEvaluationCal
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Jiegougang : Window
    {
        IDictionary<string, IList<IDictionary<string, string>>> dataExtra = null;
        IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData = null;

        public Jiegougang()
        {

            //InitializeComponent();
        }

        private void Btn_ckbg_Click(object sender, RoutedEventArgs e)
        {
            string err;

            //Calculate(dataExtra, ref retData, out err);
        }
    }
}
