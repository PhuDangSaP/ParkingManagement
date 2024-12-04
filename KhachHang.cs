using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement
{
   public  class KhachHang
    {
        public void taoKH(string ma, string ten, string cccd, string gioitinh, string sdt, string diachi, string maxe)
        {
            MaKH = ma;
            TenKH = ten;
            Cccd = cccd;
            GioiTinh = gioitinh;
            Sdt = sdt;
            DiaChi = diachi;
            MaXe = maxe;
        }
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string Cccd { get; set; }
        public string GioiTinh { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public string MaXe { get; set; }


    }
}
