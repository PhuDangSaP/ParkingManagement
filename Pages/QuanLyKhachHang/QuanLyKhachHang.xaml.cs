using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ParkingManagement.Pages.QuanLyKhachHang
{
    /// <summary>
    /// Interaction logic for QuanLyKhachHang.xaml
    /// </summary>
    public partial class QuanLyKhachHang : Page
    {
        private List<KhachHang>  DsKH = new List<KhachHang>();


        public QuanLyKhachHang()
        {
            InitializeComponent();
            LayDsKH();
            
          
     
        }
        
        private bool kiemTraKH(KhachHang khach)
        {
            if (khach.TenKH == "Nhập họ tên khách hàng" || Regex.IsMatch(khach.TenKH, @"^[\d\s]+$")) return false;
            if (khach.Cccd == "Nhập số CCCD" || Regex.IsMatch(khach.Cccd, @"^[a-zA-Z]+$")) return false;
            if (khach.Sdt == "Nhập số điện thoại" ||Regex.IsMatch(khach.Sdt, @"^[a-zA-Z]+$")) return false;

            return true;
        }
        private void LayDsKH()
        {
            DsKH.Clear();



            List<BsonDocument> list = DatabaseHandler.Instance.GetCollection("KhachHang").Find(new BsonDocument()).ToList();


            foreach (BsonDocument item in list)
            {
                string maKH = item["MaKH"].AsString;
                string tenKH = item["TenKH"].AsString;
                string cccd = item["CCCD"].AsString;
                string gioitinh = item["GioiTinh"].AsString;
                string diachi = item["DiaChi"].AsString;
                string sdt = item["SDT"].AsString;
                string maxe = item["MaXe"].AsString;


                DsKH.Add(new KhachHang { MaKH = maKH, TenKH = tenKH, Cccd = cccd, GioiTinh = gioitinh, DiaChi = diachi, MaXe = maxe, Sdt = sdt });
            }

            dtDsKH.ItemsSource = null;
            dtDsKH.ItemsSource = DsKH;

        }
       
        
        private void Thoat_Click(object sender, RoutedEventArgs e)
        {
            formKH.Visibility = Visibility.Hidden;
            panel.Visibility = Visibility.Hidden;
        }

        private void tenKH_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tenKH.Text == "Nhập họ tên khách hàng")
                tenKH.Text = string.Empty;
            tenKH.FontWeight = FontWeights.Regular;
        }

        private void tenKH_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tenKH.Text == string.Empty)
            {
                tenKH.Text = "Nhập họ tên khách hàng";
                tenKH.FontWeight = FontWeights.Thin;

            }
        }

        private void cccd_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cccd.Text == "Nhập số CCCD")
                cccd.Text = string.Empty;
            cccd.FontWeight = FontWeights.Regular;
        }

        private void cccd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cccd.Text == string.Empty)
            {
                cccd.Text = "Nhập số CCCD";
                cccd.FontWeight = FontWeights.Thin;

            }
        }

        private void maXe_GotFocus(object sender, RoutedEventArgs e)
        {
            if (maXe.Text == "Nhập mã xe")
                maXe.Text = string.Empty;
            maXe.FontWeight = FontWeights.Regular;
        }

        private void maXe_LostFocus(object sender, RoutedEventArgs e)
        {
            if (maXe.Text == string.Empty)
            {
                maXe.Text = "Nhập mã xe";
                maXe.FontWeight = FontWeights.Thin;

            }
        }

     

        private void dc_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dc.Text == "Nhập địa chỉ khách hàng")
                dc.Text = string.Empty;
            dc.FontWeight = FontWeights.Regular;
        }

       

        private void dc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dc.Text == string.Empty)
            {
                dc.Text = "Nhập địa chỉ khách hàng";
                

            }
        }

        private void sdt_GotFocus(object sender, RoutedEventArgs e)
        {
           if(sdt.Text == "Nhập số điện thoại")
                sdt.Text = string.Empty;
            
        }

        private void sdt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sdt.Text == string.Empty)
            {
                sdt.Text = "Nhập số điện thoại";
             

            }
        }

        private async void LuuKH_Click(object sender, RoutedEventArgs e)
        {

            if (tieuDe.Text == "NHẬP THÔNG TIN KHÁCH HÀNG")
            {
                var khach = new KhachHang();

                string id = (DsKH.Count + 1).ToString();
                khach.taoKH(id, tenKH.Text, cccd.Text, gt.Text, sdt.Text, dc.Text, maXe.Text);
                if (kiemTraKH(khach))
                {
                    DsKH.Add(khach);


                    await DatabaseHandler.Instance.GetCollection("KhachHang").InsertOneAsync(new BsonDocument {
                {"MaKH", id } ,
                { "TenKH", tenKH.Text },
                {"CCCD", cccd.Text },
                  {"SDT", sdt.Text },
                    {"GioiTinh", gt.Text },
                      {"DiaChi", dc.Text },
                {"MaXe", maXe.Text } });

                    LayDsKH();
                    MessageBox.Show("Thêm khách hàng thành công");
                    Thoat_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Thông tin khách hàng không hợp lệ");
                }
             
            }

            else
            {
                var khach = (KhachHang)dtDsKH.SelectedItem;

                if (kiemTraKH(khach)) {
                    // Create the filter to find the document to delete
                    var filter = Builders<BsonDocument>.Filter.Eq("MaKH", khach.MaKH);

                    // Retrieve the collection
                    var _collection = DatabaseHandler.Instance.GetCollection("KhachHang");

                    // Perform the delete operation
                    var result = await _collection.ReplaceOneAsync(filter, new BsonDocument
                { {"MaKH", khach.MaKH } ,
                { "TenKH", tenKH.Text },
                {"CCCD", cccd.Text },
                  {"SDT", sdt.Text },
                    {"GioiTinh", gt.Text },
                      {"DiaChi", dc.Text },
                {"MaXe", maXe.Text } });

                    LayDsKH();
                    MessageBox.Show("Cập nhật thành công");
                    Thoat_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Thông tin khách hàng không hợp lệ");
                }
               
            }
        
                  

            

        }
        

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Nhập mã khách hàng cần tìm")
                SearchBox.Text = string.Empty;
            SearchBox.FontWeight = FontWeights.Regular;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == string.Empty)
            {
                SearchBox.Text = "Nhập mã khách hàng cần tìm";
                

            }
        }

     
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = SearchBox.Text;
            if(!(string.IsNullOrEmpty(search)||search == "Nhập mã khách hàng cần tìm")) { 
                List<KhachHang> searchs = DsKH.FindAll(item => item.MaKH.Contains(SearchBox.Text));

                dtDsKH.ItemsSource = searchs;
            }


           

           







        }

        private void btnThemKH_Click(object sender, RoutedEventArgs e)
        {
            tieuDe.Text = "NHẬP THÔNG TIN KHÁCH HÀNG";
            formKH.Visibility = Visibility.Visible;
            panel.Visibility = Visibility.Visible;
            tenKH.FontWeight = FontWeights.Regular;
            cccd.FontWeight = FontWeights.Regular;
            sdt.FontWeight = FontWeights.Regular;
            dc.FontWeight = FontWeights.Regular;
            maXe.FontWeight = FontWeights.Regular;
            tenKH.Text = "Nhập họ tên khách hàng";
            cccd.Text = "Nhập số CCCD";
            sdt.Text = "Nhập số điện thoại";
            dc.Text = "Nhập địa chỉ khách hàng";
            maXe.Text = "Nhập mã xe";


        }

        private void dtDsKH_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tieuDe.Text = "THÔNG TIN KHÁCH HÀNG";
            formKH.Visibility = Visibility.Visible;
            panel.Visibility = Visibility.Visible;

            var khach = (KhachHang)dtDsKH.SelectedItem;

            tenKH.FontWeight = FontWeights.Regular;
            cccd.FontWeight = FontWeights.Regular;
            sdt.FontWeight = FontWeights.Regular;
            dc.FontWeight = FontWeights.Regular;
            maXe.FontWeight = FontWeights.Regular;
            tenKH.Text = khach.TenKH;
            cccd.Text = khach.Cccd;
            sdt.Text = khach.Sdt;
            dc.Text = khach.DiaChi;
            maXe.Text = khach.MaXe;
            gt.Text = khach.GioiTinh;
        }

        private async void btnXoaKH_Click(object sender, RoutedEventArgs e)
        {
            var khach = (KhachHang)dtDsKH.SelectedItem; // Ensure dtDsKH.SelectedItem is not null before casting
            DsKH.Remove(khach);
            dtDsKH.ItemsSource = null;
            dtDsKH.ItemsSource = DsKH;
            if (khach != null)
            {
                // Create the filter to find the document to delete
                var filter = Builders<BsonDocument>.Filter.Eq("MaKH", khach.MaKH);

                // Retrieve the collection
                var _collection = DatabaseHandler.Instance.GetCollection("KhachHang");

                // Perform the delete operation
                var result = await _collection.DeleteOneAsync(filter);

                // Optional: Check if the deletion was successful
                if (result.DeletedCount > 0)
                {
                    DsKH.Remove(khach);
                    dtDsKH.ItemsSource = null;
                    dtDsKH.ItemsSource = DsKH;
                    MessageBox.Show("Xóa thành công");
                }
                else
                {
                    MessageBox.Show("Xử lý thất bại");
                }
            
            }
        }

        private void reload_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            LayDsKH();
        }
    }

}
