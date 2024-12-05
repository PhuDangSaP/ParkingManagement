using MongoDB.Driver;
using MongoDB.Bson;
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

namespace ParkingManagement.Pages.QuanLyRaVao
{
    /// <summary>
    /// Interaction logic for EditChiTiet.xaml
    /// </summary>
    public partial class EditChiTiet : Window
    {
        private ChiTiet chiTiet;
        public EditChiTiet(ChiTiet chiTiet)
        {
            InitializeComponent();
            this.chiTiet = chiTiet;
            LoadData();
        }

        private void LoadData()
        {
            MaCTRaVaoTB.Text = chiTiet.MaCTRaVao;
            MaKHTB.Text = chiTiet.MaKH;
            ThoiGianVaoDB.SelectedDate = chiTiet.ThoiGianVao;
            ThoiGianRaDB.SelectedDate = chiTiet.ThoiGianRa;
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateChiTiet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("MaCTRaVao", MaCTRaVaoTB.Text);
                var update = Builders<BsonDocument>.Update
                    .Set("MaKH", MaKHTB.Text)
                    .Set("ThoiGianVao", ThoiGianVaoDB.SelectedDate ?? DateTime.Now)
                    .Set("ThoiGianRa", ThoiGianRaDB.SelectedDate);

                DatabaseHandler.Instance.GetCollection("ChiTietRaVao").UpdateOne(filter, update);

                MessageBox.Show("Cập nhật chi tiết ra/vào thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
