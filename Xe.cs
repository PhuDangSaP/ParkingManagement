using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement
{
    public class Xe
    {
        public void taoXe (string maxe, string bienso, string tenloaixe)
        {
            MaXe = maxe;
            BienSo = bienso;
            TenLoaiXe = tenloaixe;
        }

        public string MaXe { get; set; }
        public string BienSo { get; set; }
        public string TenLoaiXe { get; set; }
    }
}
