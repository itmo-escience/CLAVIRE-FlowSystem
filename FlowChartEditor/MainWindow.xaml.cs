using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace FlowChartEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Flow script (*.flow)|*.flow";
            dlg.DefaultExt = ".flow";
            if (dlg.ShowDialog().Value)
            {
                string flowCode = File.ReadAllText(dlg.FileName);
                codeTextBox.Text = flowCode;
            }
        }

        private void UpdateExected(object sender, ExecutedRoutedEventArgs e)
        {
            string code = codeTextBox.Text;
            FlowChart chart = new FlowChart();
            bool result = chart.Parse(code);
            logTextBox.Text = chart.ParseLog;

            chartImage.Source = chart.ChartImage;

            //new BitmapSource()

            //chartImage.Source.
        }
    }
}
