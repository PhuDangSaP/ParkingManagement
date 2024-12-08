using MongoDB.Bson;
using MongoDB.Driver;
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
                { "MaKH", MaKhachHangTB.Text },
                { "BienSoXe", BienSoXeTB.Text },
                { "LoaiXe", LoaiXeCBB.SelectedItem.ToString() },
                { "ThoiGianVao", DateTime.UtcNow }
            };

            DatabaseHandler.Instance.GetCollection("ChiTietRaVao").InsertOne(raVaoDocument);

            MessageBox.Show("Thông tin ra/vào đã được thêm thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }

        private void BienSoXeTB_LostFocus(object sender, RoutedEventArgs e)
        {
            string bienSoXe = BienSoXeTB.Text.Trim();

            if (!string.IsNullOrEmpty(bienSoXe))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("BienSo", bienSoXe);
                var result = DatabaseHandler.Instance.GetCollection("Xe").Find(filter).FirstOrDefault();

                if (result != null)
                {
                    MaXeTB.Text = result["MaXe"].AsString;
                    LoaiXeCBB.SelectedItem = result["TenLoaiXe"].AsString;

                    var customerFilter = Builders<BsonDocument>.Filter.Eq("MaXe", result["MaXe"].AsString);
                    var customerResult = DatabaseHandler.Instance.GetCollection("KhachHang").Find(customerFilter).FirstOrDefault();

                    if (customerResult != null)
                    {
                        MaKhachHangTB.Text = customerResult["MaKH"].AsString;
                        HoTenTB.Text = customerResult["TenKH"].AsString;
                        GioiTinhCBB.SelectedItem = customerResult["GioiTinh"].AsString;
                        DiaChiTB.Text = customerResult["DiaChi"].AsString;
                        SDTTB.Text = customerResult["SDT"].AsString;
                        CCCDTB.Text = customerResult["CCCD"].AsString;
                    }
                }
                else
                {
                    MessageBox.Show("Biển số xe không tồn tại trong hệ thống. Vui lòng nhập thông tin thủ công.",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
