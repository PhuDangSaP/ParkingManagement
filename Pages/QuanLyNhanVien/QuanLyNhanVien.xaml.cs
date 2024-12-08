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
            LoadNhanVien();
        }
        private void LoadNhanVien()
        {
            IMongoCollection<BsonDocument> collection = DatabaseHandler.Instance.GetCollection("NhanVien");
            var nhanVienDocuments = collection.Find(new BsonDocument()).ToList();
            nhanVien = nhanVienDocuments.Select(item    => new NhanVien
            {
                maNV = item["MaNV"].AsString,
                hoTen = item["HoTen"].AsString,
                gioiTinh = item["GioiTinh"].AsString,
                diaChi = item["DiaChi"].AsString,
                sDT = item["SDT"].AsString,
                cCCD = item["CCCD"].AsString,
                taiKhoan = item["TaiKhoan"].AsString,
                matKhau = item["MatKhau"].AsString,
                role = item["Role"].AsString
            }).ToList();

            dgNhanVien.ItemsSource = nhanVien;
        }

        private void dgNhanVien_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            var contextMenu = button?.ContextMenu;

            if (contextMenu != null)
            {
                // Hiển thị context menu khi nhấn chuột trái
                contextMenu.IsOpen = true;
                e.Handled = true; // Ngăn chặn sự kiện tiếp tục, để tránh các hành vi khác của chuột
            }
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

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có nhân viên nào được chọn trong DataGrid
            if (dgNhanVien.SelectedItem is NhanVien selectedNhanVien)
            {
                // Tạo context menu khi chọn nhân viên
                var contextMenu = (sender as Button).ContextMenu;
                contextMenu.IsOpen = true;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để thao tác.", "Thông Báo");
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedNhanVien = dgNhanVien.SelectedItem as NhanVien;

            if (selectedNhanVien != null)
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
            // Lấy nhân viên đã chọn từ DataGrid
            var selectedNhanVien = dgNhanVien.SelectedItem as NhanVien;

            if (selectedNhanVien != null)
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
        }


    }
}
