﻿using ParkingManagement.Pages.Login;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingManagement
{
    /// <summary>
    /// Interaction logic for Navigator.xaml
    /// </summary>
    public partial class Navigator : Page
    {
        public Navigator()
        {
            InitializeComponent();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            ContentFrame.Navigate(new System.Uri($"Pages/{btn.Tag}/{btn.Tag}.xaml",System.UriKind.Relative));
        }
        private void UserInfoButton_Click(object sender, RoutedEventArgs e)
        {
            ThongTinNhanVien dialog = new ThongTinNhanVien();
            dialog.ShowDialog();
        }
        
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Login.maNV = null;
            Login.role = null;
            NavigationService.Navigate(new System.Uri("Pages/Login/Login.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
