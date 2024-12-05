using MongoDB.Bson;
using MongoDB.Driver;
using ParkingManagement.Pages.QuanLyRaVao;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingManagement.Pages.QuanLyRaVao
{
    /// <summary>
    /// Interaction logic for QuanLyRaVao.xaml
    /// </summary>
    public partial class QuanLyRaVao : Page
    {

        private List<ChiTiet> listChiTietRaVao = new List<ChiTiet>();

        public QuanLyRaVao()
        {
            InitializeComponent();
            GetListChiTietRaVao();
            ListChiTiet.ItemsSource = listChiTietRaVao;
        }

        private void GetListChiTietRaVao()
        {
            listChiTietRaVao.Clear();
            List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("ChiTietRaVao").Find(new BsonDocument()).ToList();
            foreach (BsonDocument item in list)
            {
                string maCTRaVao = item["MaCTRaVao"].AsString;
                string maKH = item["MaKH"].AsString;
                DateTime thoiGianVao = item["ThoiGianVao"].ToUniversalTime();
                DateTime? thoiGianRa = item.Contains("ThoiGianRa") ? item["ThoiGianRa"].ToNullableUniversalTime() : null;

                listChiTietRaVao.Add(new ChiTiet
                {
                    MaCTRaVao = maCTRaVao,
                    MaKH = maKH,
                    ThoiGianVao = thoiGianVao,
                    ThoiGianRa = thoiGianRa
                });
            }
            ListChiTiet.Items.Refresh();
        }

        private void AddChiTiet_Click(object sender, RoutedEventArgs e)
        {
            AddChiTiet dialog = new AddChiTiet();
            dialog.ShowDialog();
            GetListChiTietRaVao();
        }

        private void EditChiTiet_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var filter = Builders<BsonDocument>.Filter.Eq("MaCTRaVao", btn.Tag);
            BsonDocument result = DatabaseHandler.Instance.GetCollection("ChiTietRaVao").Find(filter).FirstOrDefault();

            ChiTiet chiTiet = new ChiTiet
            {
                MaCTRaVao = result["MaCTRaVao"].AsString,
                MaKH = result["MaKH"].AsString,
                ThoiGianVao = result["ThoiGianVao"].ToUniversalTime(),
                ThoiGianRa = result.Contains("ThoiGianRa") ? result["ThoiGianRa"].ToNullableUniversalTime() : null
            };

            EditChiTiet dialog = new EditChiTiet(chiTiet);
            dialog.ShowDialog();
            GetListChiTietRaVao();
        }
    }
}
