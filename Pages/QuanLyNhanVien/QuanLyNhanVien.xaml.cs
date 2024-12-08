using MongoDB.Bson;
using MongoDB.Driver;
using ParkingManagement.Pages.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ParkingManagement.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyNhanVien.xaml
    /// </summary>
    public partial class QuanLyNhanVien : Page
    {
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
            nhanVien = nhanVienDocuments.Select(item => new NhanVien
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

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                dgNhanVien.ItemsSource = nhanVien; // Hiển thị toàn bộ danh sách
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
                    try
                    {
                        // Kết nối với MongoDB và thực hiện xóa
                        var collection = DatabaseHandler.Instance.GetCollection("NhanVien");
                        var filter = Builders<BsonDocument>.Filter.Eq("maNV", selectedNhanVien.maNV);

                        // Xóa nhân viên khỏi MongoDB
                        var deleteResult = await collection.DeleteOneAsync(filter);

                        if (deleteResult.DeletedCount > 0)
                        {
                            // Nếu xóa thành công, xóa nhân viên khỏi danh sách nhanVien
                            nhanVien.Remove(selectedNhanVien);

                            // Cập nhật lại DataGrid
                            dgNhanVien.ItemsSource = null; // Reset ItemsSource
                            dgNhanVien.ItemsSource = nhanVien; // Cập nhật lại dữ liệu mới

                            MessageBox.Show("Nhân viên đã được xóa thành công!", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để xóa.", "Lỗi");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa.", "Thông báo");
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien selectedNhanVien)
            {
                var detailsWindow = new NhanVienDetails(selectedNhanVien);
                detailsWindow.ShowDialog();
                dgNhanVien.ItemsSource = null;
                dgNhanVien.ItemsSource = nhanVien; // Cập nhật lại DataGrid sau khi chỉnh sửa
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var nhanVienMoi = new NhanVien
            {
                matKhau = "1", // Mật khẩu mặc định là "1"
                taiKhoan = string.Empty // Tài khoản sẽ được nhập sau
            };

            var addEmployeeDialog = new NhanVienDetails(nhanVienMoi);
            
            if (addEmployeeDialog.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(nhanVienMoi.maNV) || string.IsNullOrEmpty(nhanVienMoi.hoTen))
                {
                    MessageBox.Show("Mã nhân viên và họ tên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Dừng lại nếu có trường dữ liệu quan trọng thiếu
                }
                // Thêm nhân viên mới vào cơ sở dữ liệu
                var collection = DatabaseHandler.Instance.GetCollection("NhanVien");
                var newNhanVienDocument = new BsonDocument
                {
                    { "maNV", nhanVienMoi.maNV },
                    { "hoTen", nhanVienMoi.hoTen },
                    { "gioiTinh", nhanVienMoi.gioiTinh },
                    { "diaChi", nhanVienMoi.diaChi },
                    { "sdt", nhanVienMoi.sDT },
                    { "cccd", nhanVienMoi.cCCD },
                    { "taiKhoan", nhanVienMoi.maNV }, // Tài khoản mặc định là mã nhân viên
                    { "matKhau", "1" }, // Mật khẩu mặc định
                    { "role", nhanVienMoi.role }
                };
                collection.InsertOne(newNhanVienDocument); // Thêm nhân viên mới vào MongoDB

                // Cập nhật danh sách hiển thị
                nhanVien.Add(nhanVienMoi);
                //dgNhanVien.ItemsSource = null;
                dgNhanVien.ItemsSource = nhanVien;

                MessageBox.Show("Nhân viên mới đã được thêm thành công!", "Thông báo");
            }
        }
    }
}
