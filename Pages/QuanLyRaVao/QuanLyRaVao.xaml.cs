using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        private void GetListChiTietRaVao(string searchText = "")
        {
            listChiTietRaVao.Clear();
            try
            {
                var filter = new BsonDocument();

                var documents = DatabaseHandler.Instance.GetCollection("ChiTietRaVao").Find(filter).ToList();

                foreach (var doc in documents)
                {
                    string maRV = doc.GetValue("MaRV", "").ToString();
                    string maKH = doc.GetValue("MaKH", "").ToString();
                    string bienSoXe = doc.GetValue("BienSoXe", "Không rõ").ToString();
                    DateTime thoiGianVao = doc.GetValue("ThoiGianVao", DateTime.MinValue).ToUniversalTime();
                    DateTime? thoiGianRa = doc.Contains("ThoiGianRa") ? doc["ThoiGianRa"].ToNullableUniversalTime() : null;
                    string maBaiDo = doc.GetValue("MaBaiDo", "").ToString();
                    string maViTriDo = doc.GetValue("MaViTriDo", "").ToString();

                    listChiTietRaVao.Add(new ChiTietRaVao
                    {
                        maRV = maRV,
                        maKH = maKH,
                        bienSoXe = bienSoXe,
                        thoiGianVao = thoiGianVao,
                        thoiGianRa = thoiGianRa,
                        maBaiDo = maBaiDo,
                        maViTriDo = maViTriDo
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            GetListChiTietRaVao(searchText);
        }

        private void AddVehicle_Click(object sender, RoutedEventArgs e)
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
            var button = sender as Button;
            string maRV = button?.Tag.ToString();
            // Cập nhật thời gian ra logic ở đây
        }

        private void XoaRaVao_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string maRV = button?.Tag.ToString();
            // Xóa logic ở đây
        }
    }
}
