using MongoDB.Bson;
using MongoDB.Driver;
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

namespace ParkingManagement.Pages.Login
{
    /// <summary>
    /// Interaction logic for ThongTinNhanVien.xaml
    /// </summary>
    public partial class ThongTinNhanVien : Window
    {
        List<string> gioitinhs = new List<string>() { "Nam","Nữ"};
        
        public ThongTinNhanVien()
        {
            InitializeComponent();
            GioiTinhCBB.ItemsSource = gioitinhs;
            LoadData();
        }
        private void LoadData()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("MaNV", Login.maNV);
            BsonDocument result = DatabaseHandler.Instance.GetCollection("NhanVien").Find(filter).FirstOrDefault();
            //NhanVien user= new NhanVien
            //{
            //    maNV = result["MaNV"].AsString,
            //    hoTen = result["HoTen"].AsString,
            //    gioiTinh = result["GioiTinh"].AsString,
            //    diaChi = result["DiaChi"].AsString,
            //    sDT = result["SDT"].AsString,
            //    cCCD = result["CCCD"].AsString,
            //    taiKhoan = result["TaiKhoan"].AsString,
            //    matKhau = result["MatKhau"].AsString,
            //    role = result["Role"].AsString,
            //};
            MaNVTB.Text = result["MaNV"].AsString;
            HoTenTB.Text = result["HoTen"].AsString;
            GioiTinhCBB.SelectedValue = result["GioiTinh"].AsString;
            DiaChiTB.Text= result["DiaChi"].AsString;
            SDTTB.Text= result["SDT"].AsString;
            CCCDTB.Text = result["CCCD"].AsString;
            TaiKhoanTB.Text = result["TaiKhoan"].AsString;
            MatKhauTB.Text = result["MatKhau"].AsString;
            RoleTB.Text = result["Role"].AsString;
        }
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("MaNV", MaNVTB.Text);
            var update = Builders<BsonDocument>.Update
                .Set("HoTen", HoTenTB.Text)
                .Set("GioiTinh", GioiTinhCBB.SelectedValue)
                .Set("DiaChi", DiaChiTB.Text)
                .Set("SDT", SDTTB.Text)
                .Set("CCCDTB", CCCDTB.Text)
                .Set("TaiKhoan", TaiKhoanTB.Text)
                .Set("MatKhau", MatKhauTB.Text);
            DatabaseHandler.Instance.GetCollection("NhanVien").UpdateOne(filter, update);
            DialogResult = true;
        }
    }
}
