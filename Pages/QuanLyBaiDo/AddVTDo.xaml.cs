using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ParkingManagement.Pages.QuanLyBaiDo
{
    /// <summary>
    /// Interaction logic for AddVTDo.xaml
    /// </summary>
    public partial class AddVTDo : Window
    {
        private List<BaiDo> listBaiDoOto = new List<BaiDo>();
        public AddVTDo()
        {
            InitializeComponent();
            GetListBaiDo();
            BaiDoCBB.ItemsSource = listBaiDoOto;
            BaiDoCBB.SelectedIndex = 0;
        }
        private void GetListBaiDo()
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
                var countFilter = Builders<BsonDocument>.Filter.Eq("MaBD", maBD);
                int count = Convert.ToInt32(DatabaseHandler.Instance.GetCollection("ViTriDo").CountDocuments(countFilter));
                if (count < soLuong)
                {
                    listBaiDoOto.Add(new BaiDo{
                        maBD = maBD,
                        tenBD = tenBD,
                        loaiBD = loaiBD,
                        soLuong = soLuong
                    });
                }
                
            }
        }    
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddVTD_Click(object sender, RoutedEventArgs e)
        {
            BaiDo baiDo = BaiDoCBB.SelectedItem as BaiDo;
            if (baiDo != null)
            {
                DatabaseHandler.Instance.GetCollection("ViTriDo").InsertOne(new BsonDocument {
                    {"MaVTD", MaVTDTB.Text } ,
                    {"MaBD",baiDo.maBD },
                    {"TenVTD", TenVTDTB.Text },
                    {"TrangThai","Sẵn sàng"}
                });
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bãi đỗ!");
                DialogResult = false;
            }
            Close();
        }
    }
}
