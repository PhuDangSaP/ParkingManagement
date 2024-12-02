using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ParkingManagement.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyBaiDo.xaml
    /// </summary>
    public partial class QuanLyBaiDo : Page
    {
        private List<BaiDo> listBaiDoOto = new List<BaiDo>();
        private List<ViTriDo> listViTriDoOto = new List<ViTriDo>();
        public QuanLyBaiDo()
        {
            InitializeComponent();
            GetListBaiDoOto();
            GetListViTriDoOTo();
            ListVTDOto.ItemsSource = listViTriDoOto;
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
        }

    }
}
