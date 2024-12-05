using Microsoft.VisualBasic;
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
    }
}
