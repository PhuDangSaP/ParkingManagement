using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ParkingManagement.Pages.QuanLyRaVao
{
    /// <summary>
    /// Interaction logic for QuanLyRaVao.xaml
    /// </summary>
    public partial class QuanLyRaVao : Page
    {
        private List<ChiTietRaVao> listChiTietRaVao = new List<ChiTietRaVao>();

        public QuanLyRaVao()
        {
            InitializeComponent();
            GetListChiTietRaVao();
            ListChiTietRaVao.ItemsSource = listChiTietRaVao;
        }

        private void GetListChiTietRaVao()
        {
            listChiTietRaVao.Clear();
            List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("ChiTietRaVao").Find(new BsonDocument()).ToList();
            foreach (BsonDocument item in list)
            {
                string maRV = item["MaRV"].AsString;
                string maBD = item["MaBD"].AsString;
                string bienSoXe = item["BienSoXe"].AsString;
                DateTime thoiGianVao = item["ThoiGianVao"].ToUniversalTime();
                DateTime? thoiGianRa = item.Contains("ThoiGianRa") ? item["ThoiGianRa"].ToNullableUniversalTime() : null;

                listChiTietRaVao.Add(new ChiTietRaVao
                {
                    maRV = maRV,
                    maBD = maBD,
                    bienSoXe = bienSoXe,
                    thoiGianVao = thoiGianVao,
                    thoiGianRa = thoiGianRa
                });
            }
            ListChiTietRaVao.Items.Refresh();
        }

        private void AddRaVao_Click(object sender, RoutedEventArgs e)
        {
            AddRaVao dialog = new AddRaVao();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                GetListChiTietRaVao();
            }
        }

        private void CapNhatThoiGianRa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string maRV = btn.Tag.ToString();

            var filter = Builders<BsonDocument>.Filter.Eq("MaRV", maRV);
            var update = Builders<BsonDocument>.Update.Set("ThoiGianRa", DateTime.UtcNow);

            DatabaseHandler.Instance.GetCollection("ChiTietRaVao").UpdateOne(filter, update);
            MessageBox.Show("Thời gian ra đã được cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            GetListChiTietRaVao();
        }

        private void XoaRaVao_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string maRV = btn.Tag.ToString();

            var filter = Builders<BsonDocument>.Filter.Eq("MaRV", maRV);
            DatabaseHandler.Instance.GetCollection("ChiTietRaVao").DeleteOne(filter);

            MessageBox.Show("Thông tin ra vào đã được xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            GetListChiTietRaVao();
        }
    }


}
