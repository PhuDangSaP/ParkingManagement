using MongoDB.Bson;
using MongoDB.Driver;
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

namespace ParkingManagement.Pages.QuanLyBaiDo
{
    /// <summary>
    /// Interaction logic for ChiTietBaiDo.xaml
    /// </summary>
    public partial class ChiTietBaiDo : Window
    {
        private BaiDo baiDo;
        List<string> listLoaiBD = new List<string>() { "Oto", "XeMay" };
        public ChiTietBaiDo(BaiDo baiDo)
        {
            InitializeComponent();
            this.baiDo = baiDo;
            LoadData();
        }
        private void LoadData()
        {
            MaBaiDoTB.Text = baiDo.maBD;
            TenBaiDoTB.Text = baiDo.tenBD;
            LoaiBaiDoCBB.ItemsSource = listLoaiBD;
            LoaiBaiDoCBB.SelectedValue = baiDo.loaiBD;
            SoLuongTB.Text = baiDo.soLuong.ToString();
        }
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void UpdateBaiDo_Click(object sender, RoutedEventArgs e)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("MaBD", MaBaiDoTB.Text);
            var update = Builders<BsonDocument>.Update
                .Set("TenBD", TenBaiDoTB.Text) 
                .Set("SoLuong",Convert.ToInt32( SoLuongTB.Text));

            DatabaseHandler.Instance.GetCollection("BaiDo").UpdateOne(filter, update);
            DialogResult = true;
        }
    }
}
