using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using ParkingManagement.Pages.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ParkingManagement.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyNhanVien.xaml
    /// </summary>
    public partial class QuanLyNhanVien : Page
    {
        //private IMongoCollection<BsonDocument> collection;
        private List<NhanVien> nhanVien;

        public QuanLyNhanVien()
        {
            InitializeComponent();
            nhanVien = LoadNhanVien();
        }
        private List<NhanVien> LoadNhanVien()
        {
            var collection = DatabaseHandler.Instance.GetCollection("NhanViens").Find(new BsonDocument()).ToList();
            return collection.Select(item => new NhanVien
            {
                maNV = item["maNV"].AsString,
                hoTen = item["hoTen"].AsString,
                gioiTinh = item["gioiTinh"].AsString,
                diaChi = item["diaChi"].AsString,
                sDT = item["sdt"].AsString,
                cCCD = item["cccd"].AsString,
                taiKhoan = item["taiKhoan"].AsString,
                matKhau = item["matKhau"].AsString,
                role = item["role"].AsString
            }).ToList();

            dgNhanVien.ItemsSource = collection ;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                dgNhanVien.ItemsSource = nhanVien; // Hiển thị toàn bộ danh sách nếu không có từ khóa
            }
            else
            {
                var filteredList = nhanVien.Where(nv => nv.hoTen.ToLower().Contains(searchText)).ToList();
                dgNhanVien.ItemsSource = filteredList; // Hiển thị danh sách đã lọc
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien selectedNhanVien)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {selectedNhanVien.hoTen}?", "Xác nhận", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var collection = DatabaseHandler.Instance.GetCollection("NhanVien");
                    var filter = Builders<BsonDocument>.Filter.Eq("maNV", selectedNhanVien.maNV);

                    await collection.DeleteOneAsync(filter); // Xóa nhân viên khỏi MongoDB
                    nhanVien.Remove(selectedNhanVien); // Xóa nhân viên khỏi danh sách hiển thị
                    dgNhanVien.ItemsSource = nhanVien; // Cập nhật lại DataGrid
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Thông Báo");
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien selectedNhanVien)
            {
                MessageBox.Show($"Chi tiết nhân viên:\n" +
                                $"Mã NV: {selectedNhanVien.maNV}\n" +
                                $"Họ Tên: {selectedNhanVien.hoTen}\n" +
                                $"Giới Tính: {selectedNhanVien.gioiTinh}\n" +
                                $"CCCD: {selectedNhanVien.cCCD}\n" +
                                $"SĐT: {selectedNhanVien.sDT}\n" +
                                $"Địa Chỉ: {selectedNhanVien.diaChi}\n" +
                                $"Tài Khoản: {selectedNhanVien.taiKhoan}\n" +
                                $"Role: {selectedNhanVien.role}", "Thông Tin Chi Tiết");
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xem chi tiết.", "Thông Báo");
            }
        }


    }
}
