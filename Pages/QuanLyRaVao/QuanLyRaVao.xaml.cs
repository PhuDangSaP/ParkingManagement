using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ParkingManagement.Pages.QuanLyRaVao
{
    public partial class QuanLyRaVao : Page
    {
        private ObservableCollection<ChiTietRaVao> listChiTietRaVao = new ObservableCollection<ChiTietRaVao>();

        public QuanLyRaVao()
        {
            InitializeComponent();
            GetListChiTietRaVao();
            ListChiTietRaVao.ItemsSource = listChiTietRaVao;
        }

        private void GetListChiTietRaVao()
        {
            listChiTietRaVao.Clear();
            try
            {
                var documents = DatabaseHandler.Instance.GetCollection("ChiTietRaVao").Find(new BsonDocument()).ToList();

                foreach (var doc in documents)
                {
                    string maRV = doc.GetValue("MaRV", "").ToString();
                    string maKH = doc.GetValue("MaKH", "").ToString();
                    string bienSoXe = doc.GetValue("BienSoXe", "Không rõ").ToString();
                    DateTime thoiGianVao = doc.GetValue("ThoiGianVao", DateTime.MinValue).ToUniversalTime();
                    DateTime? thoiGianRa = doc.Contains("ThoiGianRa") ? doc["ThoiGianRa"].ToNullableUniversalTime() : null;

                    listChiTietRaVao.Add(new ChiTietRaVao
                    {
                        maRV = maRV,
                        maKH = maKH,
                        bienSoXe = bienSoXe,
                        thoiGianVao = thoiGianVao,
                        thoiGianRa = thoiGianRa
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            if (btn == null) return;

            string maRV = btn.Tag?.ToString();
            if (string.IsNullOrEmpty(maRV)) return;

            var filter = Builders<BsonDocument>.Filter.Eq("MaRV", maRV);
            var update = Builders<BsonDocument>.Update.Set("ThoiGianRa", DateTime.UtcNow);

            try
            {
                DatabaseHandler.Instance.GetCollection("ChiTietRaVao").UpdateOne(filter, update);
                MessageBox.Show("Thời gian ra đã được cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                GetListChiTietRaVao();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thời gian ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void XoaRaVao_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string maRV = btn.Tag?.ToString();
            if (string.IsNullOrEmpty(maRV)) return;

            var filter = Builders<BsonDocument>.Filter.Eq("MaRV", maRV);

            try
            {
                DatabaseHandler.Instance.GetCollection("ChiTietRaVao").DeleteOne(filter);
                MessageBox.Show("Thông tin ra vào đã được xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                GetListChiTietRaVao();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VehicleDetails_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string maXe = btn.Tag?.ToString();
            if (!string.IsNullOrEmpty(maXe))
            {
                MessageBox.Show($"Chi tiết phương tiện:\nMã xe: {maXe}", "Thông tin chi tiết", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã xe!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddVehicle_Click(object sender, RoutedEventArgs e)
        {
            AddRaVao addRaVaoDialog = new AddRaVao();
            bool? dialogResult = addRaVaoDialog.ShowDialog();

            if (dialogResult == true)
            {
                MessageBox.Show("Phương tiện đã được thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                GetListChiTietRaVao();
            }
        }
    }
}
