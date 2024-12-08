using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ParkingManagement.Pages.QuanLyBaiDo
{
    /// <summary>
    /// Interaction logic for ChiTietVTDo.xaml
    /// </summary>
    public partial class ChiTietVTDo : Window
    {
        private ViTriDo viTriDo;
        private List<string> listTrangThai = new List<string>() { "Sẵn sàng", "Đang hoạt động" };
        public ChiTietVTDo(ViTriDo viTriDo)
        {
            InitializeComponent();
            this.viTriDo = viTriDo;
            LoadData();
        }
        private void LoadData()
        {
            MaVTDTB.Text = viTriDo.maVTD;
            TenVTDTB.Text = viTriDo.tenVTD;

            List<BaiDo> listBaiDoOto = new List<BaiDo>();
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
            BaiDoCBB.ItemsSource = listBaiDoOto;
            foreach (BaiDo item in listBaiDoOto)
            {
                if (item.maBD == viTriDo.maBD)
                {
                    BaiDoCBB.SelectedItem = item;
                    break;
                }
            }
            TrangThaiCBB.ItemsSource = listTrangThai;
            TrangThaiCBB.SelectedValue = viTriDo.trangThai;
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DeleteVTD_Click(object sender, RoutedEventArgs e)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("MaVTD", MaVTDTB.Text);
            DatabaseHandler.Instance.GetCollection("ViTriDo").DeleteOne(filter);
            MessageBox.Show("Xóa vị trí đỗ thành công");
            DialogResult = true;
        }
        private void UpdateVTD_Click(object sender, RoutedEventArgs e)
        {
            BaiDo baiDo = BaiDoCBB.SelectedItem as BaiDo;

            var filter = Builders<BsonDocument>.Filter.Eq("MaVTD", MaVTDTB.Text);
            var update = Builders<BsonDocument>.Update
                .Set("TenVTD", TenVTDTB.Text)
                .Set("MaBD",baiDo.maBD)
                .Set("TrangThai", TrangThaiCBB.SelectedValue.ToString());
        
            DatabaseHandler.Instance.GetCollection("ViTriDo").UpdateOne(filter, update);
            DialogResult = true;
        }
    }
}
