﻿using MaterialDesignThemes.Wpf;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Intrinsics.Arm;

namespace ParkingManagement.Pages.QuanLyBaiDo
{
    /// <summary>
    /// Interaction logic for QuanLyBaiDo.xaml
    /// </summary>
    public partial class QuanLyBaiDo : Page
    {
        private List<BaiDo> listBaiDoOto = new List<BaiDo>();
        private List<BaiDo> listBaiDoXeMay = new List<BaiDo>();
        private List<BaiDo> listBaiDo = new List<BaiDo>();
        private List<ViTriDo> listViTriDoOto = new List<ViTriDo>();
     
        public QuanLyBaiDo()
        {
            InitializeComponent();

            SetPemission();

            GetListBaiDo();
            //GetListBaiDoOto();
            //GetListBaiDoXeMay();

            GetListViTriDoOTo();
            ListBaiDo.ItemsSource = listBaiDo;
            ListVTDOto.ItemsSource = listViTriDoOto;
            ListBaiDoXeMay.ItemsSource = listBaiDoXeMay;
        }

        private void SetPemission()
        {
            string role = ParkingManagement.Pages.Login.Login.role;
            if (role.Equals("Nhân viên"))
            {
                AddBaiDoBtn.IsEnabled = false;
                AddViTriDoBtn.IsEnabled = false;
            }

        private void GetListBaiDo()
        {
            listBaiDo.Clear();
            listBaiDoOto.Clear();
            listBaiDoXeMay.Clear();
            List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("BaiDo").Find(new BsonDocument()).ToList();
            foreach (BsonDocument item in list)
            {
                string maBD = item["MaBD"].AsString;
                string tenBD = item["TenBD"].AsString;
                string loaiBD = item["LoaiBD"].AsString;
                int soLuong = item["SoLuong"].ToInt32();
                string trangThai = item["TrangThai"].AsString; 
                string color = "";
                        switch (trangThai)
                        {
                            case "Sẵn sàng":
                                color = "#03fcb1";
                                break;
                            case "Đang hoạt động":
                                color = "#6bb4d1";
                                break;
                        }
                listBaiDo.Add(new BaiDo { maBD = maBD, tenBD = tenBD, loaiBD = loaiBD, soLuong = soLuong,color=color });
                switch(loaiBD)
                {
                    case "Oto":
                        listBaiDoOto.Add(new BaiDo { maBD = maBD, tenBD = tenBD, loaiBD = loaiBD, soLuong = soLuong,color=color });
                        break;
                    case "XeMay":
                        var countFilter = Builders<BsonDocument>.Filter.Eq("MaBD", maBD);
                        int soChoDangSuDung = Convert.ToInt32(DatabaseHandler.Instance.GetCollection("ChiTietRaVao").CountDocuments(countFilter));

                       
                        listBaiDoXeMay.Add(new BaiDo { maBD = maBD, tenBD = tenBD, loaiBD = loaiBD, soLuong = soLuong, trangThai = trangThai, soChoDangSuDung = soChoDangSuDung, soChoConTrong = soLuong - soChoDangSuDung, color = color });
                        break;

                }
            }
            ListBaiDo.Items.Refresh();
            ListBaiDoXeMay.Items.Refresh();
          
        }
        private void GetListBaiDoOto()
        {
            listBaiDoOto.Clear();
            var filter = Builders<BsonDocument>.Filter.Eq("LoaiBD", "Oto");
            List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("BaiDo").Find(filter).ToList();
            foreach (BsonDocument item in list)
            {
                string maBD = item["MaBD"].AsString;
                string tenBD = item["TenBD"].AsString;
                string loaiBD = item["LoaiBD"].AsString;
                int soLuong = item["SoLuong"].ToInt32();
                listBaiDoOto.Add(new BaiDo { maBD = maBD, tenBD = tenBD, loaiBD = loaiBD, soLuong = soLuong });
            }

        }
        private void GetListBaiDoXeMay()
        {
            listBaiDoXeMay.Clear();
            var filter = Builders<BsonDocument>.Filter.Eq("LoaiBD", "XeMay");
            List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("BaiDo").Find(filter).ToList();
            foreach (BsonDocument item in list)
            {
                string maBD = item["MaBD"].AsString;
                string tenBD = item["TenBD"].AsString;
                string loaiBD = item["LoaiBD"].AsString;
                int soLuong = item["SoLuong"].ToInt32();
                string trangThai = item["TrangThai"].AsString;
                var countFilter = Builders<BsonDocument>.Filter.Eq("MaBD", maBD);
                int soChoDangSuDung = Convert.ToInt32(DatabaseHandler.Instance.GetCollection("ChiTietRaVao").CountDocuments(countFilter));

                string color = "";
                switch (trangThai)
                {
                    case "Sẵn sàng":
                        color = "#03fcb1";
                        break;
                    case "Đang hoạt động":
                        color = "#6bb4d1";
                        break;
                }
                listBaiDoXeMay.Add(new BaiDo { maBD = maBD, tenBD = tenBD, loaiBD = loaiBD, soLuong = soLuong,trangThai=trangThai, soChoDangSuDung = soChoDangSuDung, soChoConTrong = soLuong - soChoDangSuDung,color=color });
            }
            ListBaiDoXeMay.Items.Refresh();
        }
        private void GetListViTriDoOTo()
        {
            listViTriDoOto.Clear();
       
            foreach (BaiDo baido in listBaiDoOto)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("MaBD", baido.maBD);
                List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("ViTriDo").Find(filter).ToList();
                foreach (BsonDocument item in list)
                {
                    string maVTD = item["MaVTD"].AsString;
                    string maBD = item["MaBD"].AsString;
                    string tenVTD = item["TenVTD"].AsString;
                    string trangThai = item["TrangThai"].AsString;

                    string color = "";
                    switch (trangThai)
                    {
                        case "Sẵn sàng":
                            color = "#03fcb1";
                            break;
                        case "Đang hoạt động":
                            color = "#6bb4d1";
                            break;
                    }
                    listViTriDoOto.Add(new ViTriDo { maVTD = maVTD, maBD = maBD, tenVTD = tenVTD, trangThai = trangThai,color=color});
                }

            }
            ListVTDOto.Items.Refresh();
        }

        private void AddBaiDo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddBaiDo dialog = new AddBaiDo();
            dialog.ShowDialog();
            GetListBaiDoOto();
            GetListBaiDoXeMay();
        }
        private void AddVTD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddVTDo dialog = new AddVTDo();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                GetListViTriDoOTo();
            }
        }

        private void VTD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var filter = Builders<BsonDocument>.Filter.Eq("MaVTD", btn.Tag);
            BsonDocument result = DatabaseHandler.Instance.GetCollection("ViTriDo").Find(filter).FirstOrDefault();
            ViTriDo viTriDo = new ViTriDo
            {
                maVTD = result["MaVTD"].AsString,
                maBD = result["MaBD"].AsString,
                tenVTD = result["TenVTD"].AsString,
                trangThai = result["TrangThai"].AsString
            };
            switch (viTriDo.trangThai)
            {
                case "Sẵn sàng":
                    ChiTietVTDo chiTietVTDo = new ChiTietVTDo(viTriDo);
                    chiTietVTDo.ShowDialog();
                    GetListViTriDoOTo();
                    break;
                case "Đang hoạt động":
                    MessageBox.Show("Không thể chỉnh sửa vị trí đỗ đang hoạt động");
                    break;
            }

        }
        private void BaiDo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var filter = Builders<BsonDocument>.Filter.Eq("MaBD", btn.Tag);
            BsonDocument doc = DatabaseHandler.Instance.GetCollection("BaiDo").Find(filter).FirstOrDefault();
            BaiDo baiDo = new BaiDo
            {
                maBD = doc["MaBD"].AsString,
                tenBD = doc["TenBD"].AsString,
                loaiBD = doc["LoaiBD"].AsString,
                soLuong = doc["SoLuong"].ToInt32(),
                trangThai = doc["TrangThai"].AsString,
            };
            switch (baiDo.trangThai)
            {
                case "Sẵn sàng":
                    ChiTietBaiDo dialog = new ChiTietBaiDo(baiDo);
                    bool? result = dialog.ShowDialog();
                    if (result == true)
                    {
                        //GetListBaiDoOto();
                        //GetListBaiDoXeMay();
                        GetListBaiDo();
                    }
                    break;
                case "Đang hoạt động":
                    MessageBox.Show("Không thể chỉnh sửa bãi đỗ đang hoạt động");
                    break;
            }



        }
    }
}
