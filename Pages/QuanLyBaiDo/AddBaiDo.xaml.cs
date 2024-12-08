using MongoDB.Bson;
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
    /// Interaction logic for AddBaiDo.xaml
    /// </summary>
    public partial class AddBaiDo : Window
    {
        List<string> listLoaiBD = new List<string>() { "Oto","XeMay"};   
        public AddBaiDo()
        {
            InitializeComponent();
            LoaiBaiDoCBB.ItemsSource = listLoaiBD;
            LoaiBaiDoCBB.SelectedIndex = 0;
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBaiDo_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.Instance.GetCollection("BaiDo").InsertOne(new BsonDocument { 
                {"MaBD", MaBaiDoTB.Text } ,
                { "TenBD", TenBaiDoTB.Text },
                {"LoaiBD",LoaiBaiDoCBB.SelectedItem.ToString() },
                {"SoLuong", Convert.ToInt32(SoLuongTB.Text)},
                {"TrangThai","Sẵn sàng" }
            });
            MessageBox.Show("Thêm bãi đỗ thành công");
            Close();
        }
    }
}
