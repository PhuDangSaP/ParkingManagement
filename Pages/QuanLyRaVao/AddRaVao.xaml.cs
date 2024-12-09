using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            if (string.IsNullOrEmpty(MaXeTB.Text) || string.IsNullOrEmpty(MaKhachHangTB.Text) ||
                string.IsNullOrEmpty(HoTenTB.Text) || string.IsNullOrEmpty(DiaChiTB.Text) ||
                string.IsNullOrEmpty(BienSoXeTB.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var xeCollection = DatabaseHandler.Instance.GetCollection("Xe");
            var khachHangCollection = DatabaseHandler.Instance.GetCollection("KhachHang");
            var chiTietRaVaoCollection = DatabaseHandler.Instance.GetCollection("ChiTietRaVao");

            var newXe = new BsonDocument
            {
                { "MaXe", MaXeTB.Text.Trim() },
                { "BienSo", BienSoXeTB.Text.Trim() },
                { "TenLoaiXe", LoaiXeCBB.SelectedItem?.ToString() ?? "Không rõ" }
            };
            xeCollection.InsertOne(newXe);

            var newKhachHang = new BsonDocument
            {
                { "MaKH", MaKhachHangTB.Text.Trim() },
                { "MaXe", MaXeTB.Text.Trim() },
                { "TenKH", HoTenTB.Text.Trim() },
                { "GioiTinh", GioiTinhCBB.SelectedItem is ComboBoxItem selectedItem ? selectedItem.Content.ToString() : "Không rõ" },
                { "DiaChi", DiaChiTB.Text.Trim() },
                { "SDT", SDTTB.Text.Trim() },
                { "CCCD", CCCDTB.Text.Trim() }
            };
            khachHangCollection.InsertOne(newKhachHang);

            string maRV = MaRaVaoTB.Text.Trim();
            if (string.IsNullOrEmpty(maRV))
            {
                maRV = Guid.NewGuid().ToString();
            }

            string maBaiDo = string.Empty;
            if (LoaiXeCBB.SelectedItem?.ToString() == "Ô tô")
            {
                maBaiDo = "VTD_" + Guid.NewGuid().ToString();
            }
            else if (LoaiXeCBB.SelectedItem?.ToString() == "Xe máy")
            {
                maBaiDo = "BD_" + Guid.NewGuid().ToString();

                var newChiTietRaVao = new BsonDocument
            {
                { "MaRV", maRV },
                { "MaKH", MaKhachHangTB.Text.Trim() },
                { "BienSo", BienSoXeTB.Text.Trim() },
                { "ThoiGianVao", DateTime.UtcNow },
                { "MaBaiDo", maBaiDo }
            };
                chiTietRaVaoCollection.InsertOne(newChiTietRaVao);

                MessageBox.Show("Thông tin đã được lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void BienSoXeTB_LostFocus(object sender, RoutedEventArgs e)
        {
            string bienSoXe = BienSoXeTB.Text.Trim();

            if (!string.IsNullOrEmpty(bienSoXe))
            {
                var xeCollection = DatabaseHandler.Instance.GetCollection("Xe");
                var khachHangCollection = DatabaseHandler.Instance.GetCollection("KhachHang");

                var xeFilter = Builders<BsonDocument>.Filter.Eq("BienSo", bienSoXe);
                var xeResult = xeCollection.Find(xeFilter).FirstOrDefault();

                if (xeResult != null)
                {
                    MaXeTB.Text = xeResult["MaXe"].AsString;
                    LoaiXeCBB.SelectedItem = xeResult["TenLoaiXe"].AsString;

                    var khachHangFilter = Builders<BsonDocument>.Filter.Eq("MaXe", xeResult["MaXe"].AsString);
                    var khachHangResult = khachHangCollection.Find(khachHangFilter).FirstOrDefault();

                    if (khachHangResult != null)
                    {
                        MaKhachHangTB.Text = khachHangResult["MaKH"].AsString;
                        HoTenTB.Text = khachHangResult["TenKH"].AsString;
                        GioiTinhCBB.SelectedItem = khachHangResult["GioiTinh"].AsString;
                        DiaChiTB.Text = khachHangResult["DiaChi"].AsString;
                        SDTTB.Text = khachHangResult["SDT"].AsString;
                        CCCDTB.Text = khachHangResult["CCCD"].AsString;
                    }
                }
                else
                {
                    MessageBox.Show("Biển số xe này chưa tồn tại trong hệ thống. Vui lòng nhập mã xe và thông tin khách hàng.",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    MaXeTB.IsEnabled = true;
                    MaKhachHangTB.IsEnabled = true;
                }
            }
        }
    }
}
