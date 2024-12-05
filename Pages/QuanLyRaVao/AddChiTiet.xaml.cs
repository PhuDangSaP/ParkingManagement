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
using MongoDB.Driver;
using MongoDB.Bson;

namespace ParkingManagement.Pages.QuanLyRaVao
{
    /// <summary>
    /// Interaction logic for AddChiTiet.xaml
    /// </summary>
    public partial class AddChiTiet : Window
    {
        List<string> listLoaiXe = new List<string>() { "Oto", "XeMay" };

        public AddChiTiet()
        {
            InitializeComponent();
            LoaiXeCBB.ItemsSource = listLoaiXe;
            LoaiXeCBB.SelectedIndex = 0;
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddChiTiet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var chiTiet = new ChiTiet
                {
                    MaCTRaVao = MaCTRaVaoTB.Text.Trim(),
                    MaKH = MaKHTB.Text.Trim(),
                    ThoiGianVao = ThoiGianVaoDB.SelectedDate ?? DateTime.Now, 
                    ThoiGianRa = ThoiGianRaDB.SelectedDate 
                };

                if (string.IsNullOrEmpty(chiTiet.MaCTRaVao) || string.IsNullOrEmpty(chiTiet.MaKH))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var collection = DatabaseHandler.Instance.GetCollection("ChiTietRaVao");
                collection.InsertOne(chiTiet);

                MessageBox.Show("Đã thêm chi tiết ra/vào thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
