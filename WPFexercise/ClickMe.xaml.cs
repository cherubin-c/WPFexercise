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

namespace WPFexercise
{
    /// <summary>
    /// Interaction logic for ClickMe.xaml
    /// </summary>
    public partial class ClickMe : Window
    {
        public ClickMe()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello Worldline! WPF");
        }
    }
}
