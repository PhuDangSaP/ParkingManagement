using MongoDB.Bson;
using MongoDB.Driver;
using ParkingManagement.Pages.Login;
using System.Collections.Generic;
using System.Windows;

namespace ParkingManagement.Pages
{
    public partial class NhanVienDetails : Window
    {
        public NhanVien ExistingNhanVien { get; set; }

        List<string> gioitinhs = new List<string>() { "Nam", "Nữ" };
        public NhanVienDetails(NhanVien nhanVien)
        {
            InitializeComponent();
            GioiTinhCBB.ItemsSource = gioitinhs;
            // Gán giá trị nhân viên vào các TextBlock
            MaNVTB.Text = nhanVien.maNV;
            HoTenTB.Text = nhanVien.hoTen;
            GioiTinhCBB.Text = nhanVien.gioiTinh;
            CCCDTB.Text = nhanVien.cCCD;
            SDTTB.Text = nhanVien.sDT;
            DiaChiTB.Text = nhanVien.diaChi;
            RoleTB.Text = nhanVien.role;
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var collection = DatabaseHandler.Instance.GetCollection("NhanVien");

            if (string.IsNullOrEmpty(ExistingNhanVien?.maNV)) // Kiểm tra nếu đây là thêm mới
            {
                // Tạo nhân viên mới
                var newNhanVien = new NhanVien
                {
                    maNV = MaNVTB.Text,
                    hoTen = HoTenTB.Text,
                    gioiTinh = GioiTinhCBB.Text,
                    cCCD = CCCDTB.Text,
                    sDT = SDTTB.Text,
                    diaChi = DiaChiTB.Text,
                    taiKhoan = MaNVTB.Text, // Tài khoản mặc định là mã nhân viên
                    matKhau = "1",          // Mật khẩu mặc định là "1"
                    role = RoleTB.Text
                };

                var newNhanVienBson = new BsonDocument
            {
                { "MaNV", newNhanVien.maNV },
                { "HoTen", newNhanVien.hoTen },
                { "GioiTinh", newNhanVien.gioiTinh },
                { "DiaChi", newNhanVien.diaChi },
                { "SDT", newNhanVien.sDT },
                { "CCCD", newNhanVien.cCCD },
                { "TaiKhoan", newNhanVien.taiKhoan },
                { "MatKhau", newNhanVien.matKhau },
                { "Role", newNhanVien.role }
            };

                await collection.InsertOneAsync(newNhanVienBson);
                MessageBox.Show("Thêm nhân viên mới thành công!", "Thông báo");
            }
                var filter = Builders<BsonDocument>.Filter.Eq("MaNV", MaNVTB.Text);
            var update = Builders<BsonDocument>.Update
                .Set("HoTen", HoTenTB.Text)
                .Set("GioiTinh", GioiTinhCBB.SelectedValue)
                .Set("DiaChi", DiaChiTB.Text)
                .Set("SDT", SDTTB.Text)
                .Set("CCCD", CCCDTB.Text);
                //.Set("TaiKhoan", TaiKhoanTB.Text)
                //.Set("MatKhau", MatKhauTB.Text);
            DatabaseHandler.Instance.GetCollection("NhanVien").UpdateOne(filter, update);
            DialogResult = true;
        }
    }
}
