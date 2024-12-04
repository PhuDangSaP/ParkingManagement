using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ParkingManagement.Pages.QuanLyBaiDo
{
    /// <summary>
    /// Interaction logic for QuanLyBaiDo.xaml
    /// </summary>
    public partial class QuanLyBaiDo : Page
    {
        private List<BaiDo> listBaiDoOto = new List<BaiDo>();
        private List<BaiDo> listBaiDoXeMay = new List<BaiDo>();
        private List<ViTriDo> listViTriDoOto = new List<ViTriDo>();
        public QuanLyBaiDo()
        {
            InitializeComponent();
            GetListBaiDoOto();
            GetListBaiDoXeMay();
            GetListViTriDoOTo();
            ListVTDOto.ItemsSource = listViTriDoOto;
            ListBaiDoXeMay.ItemsSource = listBaiDoXeMay;
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

                var countFilter = Builders<BsonDocument>.Filter.Eq("MaBD", maBD);
                int soChoDangSuDung = Convert.ToInt32(DatabaseHandler.Instance.GetCollection("ChiTietRaVao").CountDocuments(countFilter));
                listBaiDoXeMay.Add(new BaiDo { maBD = maBD, tenBD = tenBD, loaiBD = loaiBD, soLuong = soLuong, soChoDangSuDung=soChoDangSuDung,soChoConTrong = soLuong - soChoDangSuDung });
            }
            ListBaiDoXeMay.Items.Refresh();
        }    
        private void GetListViTriDoOTo()
        {
            listViTriDoOto.Clear();
            foreach(BaiDo baido in  listBaiDoOto)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("MaBD", baido.maBD);
                List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("ViTriDo").Find(filter).ToList();
                foreach (BsonDocument item in list)
                {
                    string maVTD = item["MaVTD"].AsString;
                    string maBD = item["MaBD"].AsString;
                    string tenVTD = item["TenVTD"].AsString;
                    string trangThai = item["TrangThai"].AsString;
                    listViTriDoOto.Add(new ViTriDo {maVTD=maVTD,maBD=maBD,tenVTD=tenVTD,trangThai=trangThai });
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
            ChiTietVTDo chiTietVTDo = new ChiTietVTDo(viTriDo);
            chiTietVTDo.ShowDialog();
            GetListViTriDoOTo();
        }
    }
}
