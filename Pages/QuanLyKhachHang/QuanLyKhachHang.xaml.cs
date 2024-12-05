using ParkingManagement.asset.dialogs;
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

namespace ParkingManagement.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyKhachHang.xaml
    /// </summary>
    public partial class QuanLyKhachHang : Page
    {
        private List<KhachHang>  DsKH = new List<KhachHang>();
        private int stt = 0;
        ThemKH form = new ThemKH();
        public QuanLyKhachHang()
        {
            InitializeComponent();
            LayDsKH();

            dtDsKH.ItemsSource = DsKH;
     
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

        private void themKH()
        {

            KhachHang khach = new KhachHang();
            khach.taoKH("004", "Nguyen Van Dong", "000000000000", "Nam", "0933386019", "19/2 Tran Quang Khai", "A304");
            DsKH.Add(khach);

            dtDsKH.ItemsSource = null;
            dtDsKH.ItemsSource = DsKH;


        }
        private void ThemKhachHang_MouseDown(object sender, MouseButtonEventArgs e)
        {
            formThemKH.Visibility = Visibility.Visible;
            main.Visibility = Visibility.Hidden;

          

            
        }

   

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string search = SearchBox.Text;
            List<KhachHang> searchs = new List<KhachHang>();
            searchs = DsKH.FindAll(item => item.MaKH.Contains( search));

            dtDsKH.ItemsSource = searchs;

        }

    
    }

}
