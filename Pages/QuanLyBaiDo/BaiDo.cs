﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement
{
    public class BaiDo
    {
        public string maBD { get; set; }
        public string tenBD { get; set; }
        public string loaiBD { get; set; }
        public int soLuong { get; set; }
        public int soChoDangSuDung { get; set; }
        public int soChoConTrong { get; set; }
        public override string ToString()
        {
            return tenBD; 
        }
    }
}
