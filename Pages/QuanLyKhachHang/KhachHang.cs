using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Pages.QuanLyKhachHang
{
    public class KhachHang
    {
        public void taoKH(string ma, string ten, string cccd, string gioitinh, string sdt, string diachi, string bien, string tenxe)
        {
            MaKH = ma;
            TenKH = ten;
            Cccd = cccd;
            GioiTinh = gioitinh;
            Sdt = sdt;
            DiaChi = diachi;
            BienSo = bien;
            TenLoaiXe = tenxe;
        }
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string Cccd { get; set; }
        public string GioiTinh { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public string BienSo { get; set; }

        public string TenLoaiXe { get; set; }
    }
    }
