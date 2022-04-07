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
using WPFexercise.Model;
using WPFexercise.Service;

namespace WPFexercise
{
    /// <summary>
    /// Interaction logic for WPFform.xaml
    /// </summary>
    public partial class WPFform : Window
    {
        UserInfoServices _userInfoServices;
        public WPFform()
        {
            InitializeComponent();
        }

        public void Start()
        {
            _userInfoServices = new UserInfoServices();
            DataContext = _userInfoServices;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.Insert();
            TextBoxFirstName.Focus();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.Update();
        }

        private void ButtonFirst_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.First();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.Previous();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.Next();
        }
        private void ButtonLast_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.Last();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.Save();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _userInfoServices.StopEditing();
        }
    }
}
