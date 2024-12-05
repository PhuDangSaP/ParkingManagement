using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement
{
    public class NhanVien
    {
        public void taoNV(string maNV, string hoTen, string gioiTinh, string diaChi, string sdt, string cccd, string taiKhoan, string matKhau, string role)
        {
            MaNV = maNV;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            SDT = sdt;
            CCCD = cccd;
            TaiKhoan = taiKhoan;
            MatKhau = matKhau;
            Role = role;
        }
        public string MaNV { get; set; }       
        public string HoTen { get; set; }      
        public string GioiTinh { get; set; }   
        public string DiaChi { get; set; }     
        public string SDT { get; set; }        
        public string CCCD { get; set; }       
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string Role { get; set; } 
    }
}
