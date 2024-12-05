using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement
{
    public class Xe
    {
        public string BienSo { get; set; }
        public string LoaiXe { get; set; }
        public string MaVTD { get; set; }
        public DateTime ThoiGianVao { get; set; }
        public DateTime? ThoiGianRa { get; set; }
    }
}
