﻿using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
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
                MaNV = item["maNV"].AsString,
                HoTen = item["hoTen"].AsString,
                GioiTinh = item["gioiTinh"].AsString,
                DiaChi = item["diaChi"].AsString,
                SDT = item["sdt"].AsString,
                CCCD = item["cccd"].AsString,
                TaiKhoan = item["taiKhoan"].AsString,
                MatKhau = item["matKhau"].AsString,
                Role = item["role"].AsString
            }).ToList();
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
                var filteredList = nhanVien.Where(nv => nv.HoTen.ToLower().Contains(searchText)).ToList();
                dgNhanVien.ItemsSource = filteredList; // Hiển thị danh sách đã lọc
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien selectedNhanVien)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {selectedNhanVien.HoTen}?", "Xác nhận", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var collection = DatabaseHandler.Instance.GetCollection("NhanVien");
                    var filter = Builders<BsonDocument>.Filter.Eq("maNV", selectedNhanVien.MaNV);

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
<<<<<<< Updated upstream
                MessageBox.Show($"Chi tiết nhân viên:\n" +
                                $"Mã NV: {selectedNhanVien.MaNV}\n" +
                                $"Họ Tên: {selectedNhanVien.HoTen}\n" +
                                $"Giới Tính: {selectedNhanVien.GioiTinh}\n" +
                                $"CCCD: {selectedNhanVien.CCCD}\n" +
                                $"SĐT: {selectedNhanVien.SDT}\n" +
                                $"Địa Chỉ: {selectedNhanVien.DiaChi}\n" +
                                $"Tài Khoản: {selectedNhanVien.TaiKhoan}\n" +
                                $"Chức vụ: {selectedNhanVien.Role}", "Thông Tin Chi Tiết");
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xem chi tiết.", "Thông Báo");
            }
        }
=======
                var detailsWindow = new NhanVienDetails(selectedNhanVien)
                {
                    ExistingNhanVien = selectedNhanVien // Truyền nhân viên hiện tại để chỉnh sửa
                };
                detailsWindow.ShowDialog();
                dgNhanVien.ItemsSource = null;
                dgNhanVien.ItemsSource = nhanVien;
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var nhanVienMoi = new NhanVien();
            var addEmployeeDialog = new NhanVienDetails(nhanVienMoi)
            {
                ExistingNhanVien = null // Thêm mới => không truyền nhân viên nào
            };

            if (addEmployeeDialog.ShowDialog() == true) // Nếu thêm thành công
            {
                // Cập nhật danh sách hiển thị
                nhanVien.Add(nhanVienMoi);
                dgNhanVien.ItemsSource = null; // Reset source để DataGrid cập nhật
                dgNhanVien.ItemsSource = nhanVien;

                MessageBox.Show("Nhân viên mới đã được thêm vào danh sách!", "Thông báo");
            }
        }

>>>>>>> Stashed changes
    }
}
