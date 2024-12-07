using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ParkingManagement.Pages.QuanLyRaVao
{
    /// <summary>
    /// Interaction logic for AddRaVao.xaml
    /// </summary>
    public partial class AddRaVao : Window
    {
        List<string> listLoaiXe = new List<string>() { "Ô tô", "Xe máy" };

        public AddRaVao()
        {
            InitializeComponent();
            LoaiXeCBB.ItemsSource = listLoaiXe;
            LoaiXeCBB.SelectedIndex = 0;
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddRaVao_Click(object sender, RoutedEventArgs e)
        {
            var raVaoDocument = new BsonDocument
            {
                { "MaRV", MaRaVaoTB.Text },
                { "MaBD", MaBaiDoTB.Text },
                { "BienSoXe", BienSoXeTB.Text },
                { "LoaiXe", LoaiXeCBB.SelectedItem.ToString() },
                { "ThoiGianVao", DateTime.UtcNow }
            };

            DatabaseHandler.Instance.GetCollection("ChiTietRaVao").InsertOne(raVaoDocument);

            MessageBox.Show("Thông tin ra/vào đã được thêm thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }
    }
}
