using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ParkingManagement.Pages
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
            
            dtDsKH.ItemsSource = DsKH;
     
        }
        
        private bool kiemTraKH(KhachHang khach)
        {
            if (khach.TenKH == "Nhập họ tên khách hàng" || Regex.IsMatch(khach.TenKH, @"^[\d\s]+$")) return false;
            return true;
        }
        private void LayDsKH()
        {
            DsKH.Clear();
            KhachHang Kh1 = new KhachHang();
            KhachHang Kh2 = new KhachHang();
            KhachHang Kh3 = new KhachHang();
            KhachHang Kh4 = new KhachHang();

            Kh1.taoKH("001", "Dang Huu Thang", "000000000000", "Nam", "0933386019", "19/2 Tran Quang Khai", "A304");
            DsKH.Add(Kh1);

            Kh2.taoKH("002", "Nguyen Thi Thanh Hang", "000000000000", "Nu", "0933386019", "19/2 Tran Quang Khai", "A304");
            DsKH.Add(Kh2);

            Kh3.taoKH("003", "Trinh Thi Thu", "000000000000", "Nu", "0933386019", "19/2 Tran Quang Khai", "A304");
            DsKH.Add(Kh3);

            Kh4.taoKH("004", "Nguyen Van Dong", "000000000000", "Nam", "0933386019", "19/2 Tran Quang Khai", "A304");
            DsKH.Add(Kh4);

            

        }

        private int themKH()
        {

            KhachHang khach = new KhachHang();

            string id = (DsKH.Count + 1).ToString();


            khach.taoKH(id, tenKH.Text, cccd.Text, gt.Text, sdt.Text, dc.Text, maXe.Text);

            if (kiemTraKH(khach))
            {
                DsKH.Add(khach);

                dtDsKH.ItemsSource = null;
                dtDsKH.ItemsSource = DsKH;

                return 1;
            }
            return 0;
            


        }
        

    

        private void TimKh_Click(object sender, RoutedEventArgs e)
        {
            string search = SearchBox.Text;
            if (!(search == string.Empty || search == "Nhập mã khách hàng cần tìm")) {
                List<KhachHang> searchs = new List<KhachHang>();
                searchs = DsKH.FindAll(item => item.MaKH.Contains(search));

                dtDsKH.ItemsSource = searchs;
            }
        
        }

        private void ThemKH_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemKhachHang.Visibility = Visibility.Visible;
            panel.Visibility = Visibility.Visible;
        }

        private void Thoat_Click(object sender, RoutedEventArgs e)
        {
            ThemKhachHang.Visibility = Visibility.Hidden;
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
                dc.FontWeight = FontWeights.Thin;

            }
        }

        private void sdt_GotFocus(object sender, RoutedEventArgs e)
        {
           if(sdt.Text == "Nhập số điện thoại")
                sdt.Text = string.Empty;
            sdt.FontWeight = FontWeights.Regular;
        }

        private void sdt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sdt.Text == string.Empty)
            {
                sdt.Text = "Nhập số điện thoại";
                sdt.FontWeight = FontWeights.Thin;

            }
        }

        private void LuuKH_Click(object sender, RoutedEventArgs e)
        {
            if (themKH() == 0) MessageBox.Show("Xử lý không thành công");
            else
            {
                MessageBox.Show("Thêm thành công");
                Thoat_Click(sender, e);
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
                SearchBox.FontWeight = FontWeights.Thin;

            }
        }

        private void XoaKH_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = dtDsKH.SelectedItem as KhachHang;

            DsKH.Remove(selectedRow);

            dtDsKH.ItemsSource = null;
            dtDsKH.ItemsSource = DsKH;
        }
    }

}
