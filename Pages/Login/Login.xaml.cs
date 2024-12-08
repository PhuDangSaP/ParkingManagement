using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace ParkingManagement.Pages.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public static string maNV;
        public static string role;

        public Login()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            IMongoCollection<BsonDocument> userCollection = DatabaseHandler.Instance.GetCollection("NhanVien");
            var filter = Builders<BsonDocument>.Filter.Eq("TaiKhoan", TaiKhoanTB.Text);
            BsonDocument user = userCollection.Find(filter).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Tên tài khoản không chính xác!");
            }
            else
            {
                if (MatKhauTB.Password == user["MatKhau"].AsString)
                {
                    
                    maNV = user["MaNV"].AsString;
                    role = user["Role"].AsString;
                    NavigationService.Navigate(new System.Uri("Navigator.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    MessageBox.Show("Mật khẩu không chính xác!");
                }
            }

        }
        
    }
}
