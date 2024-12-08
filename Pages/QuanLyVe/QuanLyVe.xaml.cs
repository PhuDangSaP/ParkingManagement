using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParkingManagement.Pages
{
    public partial class QuanLyVe : Page
    {
        private List<Ve> ticketList = new List<Ve>();

        public QuanLyVe()
        {
            InitializeComponent();
            LoadTickets();
        }

        private async void LoadTickets()
        {
            try
            {
                var collection = DatabaseHandler.Instance.GetCollection("Ve");
                var documents = await collection.Find(new BsonDocument()).ToListAsync();
                ticketList = documents.Select(doc => new Ve
                {
                    Id = doc["_id"].AsObjectId,
                    MaVe = doc["MaVe"].AsString,
                    MaLoaiVe = doc["MaLoaiVe"].AsString,
                    MaKhachHang = doc["MaKhachHang"].AsString,
                    NgayKichHoat = doc["NgayKichHoat"].ToUniversalTime(),
                    TinhTrang = doc["TinhTrang"].AsString
                }).ToList();
                UpdateTicketGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTicketGrid()
        {
            TicketsDataGrid.ItemsSource = null;
            TicketsDataGrid.ItemsSource = ticketList;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchBox.Text.ToLower();

            var filteredTickets = ticketList
                .Where(ticket => ticket.MaVe.ToLower().Contains(searchQuery) ||
                                 ticket.MaLoaiVe.ToLower().Contains(searchQuery) ||
                                 ticket.MaKhachHang.ToLower().Contains(searchQuery))
                .ToList();

            TicketsDataGrid.ItemsSource = filteredTickets;
        }

        private async void AddTicket_Click(object sender, RoutedEventArgs e)
        {
            var newTicket = ShowTicketDialog(new Ve());
            if (newTicket != null)
            {
                try
                {
                    var collection = DatabaseHandler.Instance.GetCollection("Ve");
                    var document = new BsonDocument
                    {
                        { "MaVe", newTicket.MaVe },
                        { "MaLoaiVe", newTicket.MaLoaiVe },
                        { "MaKhachHang", newTicket.MaKhachHang },
                        { "NgayKichHoat", newTicket.NgayKichHoat },
                        { "TinhTrang", newTicket.TinhTrang }
                    };
                    await collection.InsertOneAsync(document);
                    LoadTickets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi thêm vé: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void EditTicket_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsDataGrid.SelectedItem is Ve selectedTicket)
            {
                var updatedTicket = ShowTicketDialog(selectedTicket);
                if (updatedTicket != null)
                {
                    try
                    {
                        var collection = DatabaseHandler.Instance.GetCollection("Ve");
                        var filter = Builders<BsonDocument>.Filter.Eq("_id", selectedTicket.Id);
                        var update = Builders<BsonDocument>.Update
                            .Set("MaVe", updatedTicket.MaVe)
                            .Set("MaLoaiVe", updatedTicket.MaLoaiVe)
                            .Set("MaKhachHang", updatedTicket.MaKhachHang)
                            .Set("NgayKichHoat", updatedTicket.NgayKichHoat)
                            .Set("TinhTrang", updatedTicket.TinhTrang);

                        await collection.UpdateOneAsync(filter, update);
                        LoadTickets();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra khi chỉnh sửa vé: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void DeleteTicket_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsDataGrid.SelectedItem is Ve selectedTicket)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa vé {selectedTicket.MaVe} không?",
                                             "Xác nhận xóa",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var collection = DatabaseHandler.Instance.GetCollection("Ve");
                        var filter = Builders<BsonDocument>.Filter.Eq("_id", selectedTicket.Id);
                        await collection.DeleteOneAsync(filter);
                        LoadTickets();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra khi xóa vé: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private Ve ShowTicketDialog(Ve ticket)
        {
            var originalId = ticket.Id;

            Window dialog = new Window
            {
                Title = ticket.MaVe == null ? "Thêm vé mới" : "Chỉnh sửa vé",
                Width = 400,
                Height = 450,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize
            };

            Grid grid = new Grid { Margin = new Thickness(20) };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            for (int i = 0; i < 5; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            }
            grid.Children.Add(new Label { Content = "Mã vé", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[grid.Children.Count - 1], 0);
            Grid.SetColumn(grid.Children[grid.Children.Count - 1], 0);

            var txtMaVe = new TextBox { Text = ticket.MaVe, Margin = new Thickness(5) };
            grid.Children.Add(txtMaVe);
            Grid.SetRow(txtMaVe, 0);
            Grid.SetColumn(txtMaVe, 1);

            grid.Children.Add(new Label { Content = "Mã loại vé", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[grid.Children.Count - 1], 1);
            Grid.SetColumn(grid.Children[grid.Children.Count - 1], 0);

            var txtMaLoaiVe = new TextBox { Text = ticket.MaLoaiVe, Margin = new Thickness(5) };
            grid.Children.Add(txtMaLoaiVe);
            Grid.SetRow(txtMaLoaiVe, 1);
            Grid.SetColumn(txtMaLoaiVe, 1);
            grid.Children.Add(new Label { Content = "Mã khách hàng", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[grid.Children.Count - 1], 2);
            Grid.SetColumn(grid.Children[grid.Children.Count - 1], 0);

            var txtMaKhachHang = new TextBox { Text = ticket.MaKhachHang, Margin = new Thickness(5) };
            grid.Children.Add(txtMaKhachHang);
            Grid.SetRow(txtMaKhachHang, 2);
            Grid.SetColumn(txtMaKhachHang, 1);


            grid.Children.Add(new Label { Content = "Ngày kích hoạt", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[grid.Children.Count - 1], 3);
            Grid.SetColumn(grid.Children[grid.Children.Count - 1], 0);

            var dpNgayKichHoat = new DatePicker { SelectedDate = ticket.NgayKichHoat, Margin = new Thickness(5) };
            grid.Children.Add(dpNgayKichHoat);
            Grid.SetRow(dpNgayKichHoat, 3);
            Grid.SetColumn(dpNgayKichHoat, 1);


            grid.Children.Add(new Label { Content = "Tình trạng", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[grid.Children.Count - 1], 4);
            Grid.SetColumn(grid.Children[grid.Children.Count - 1], 0);

            var cbTinhTrang = new ComboBox
            {
                Margin = new Thickness(5),
                ItemsSource = new[] { "Đang sử dụng", "Hết hạn" },
                SelectedItem = ticket.TinhTrang
            };
            grid.Children.Add(cbTinhTrang);
            Grid.SetRow(cbTinhTrang, 4);
            Grid.SetColumn(cbTinhTrang, 1);


            var btnSave = new Button
            {
                Content = "Lưu",
                Width = 80,
                Height = 30,
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidColorBrush(Colors.Purple),
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold
            };
            btnSave.Click += (s, ev) =>
            {
                ticket.Id = originalId;
                ticket.MaVe = txtMaVe.Text;
                ticket.MaLoaiVe = txtMaLoaiVe.Text;
                ticket.MaKhachHang = txtMaKhachHang.Text;
                ticket.NgayKichHoat = dpNgayKichHoat.SelectedDate ?? DateTime.Now;
                ticket.TinhTrang = cbTinhTrang.SelectedItem.ToString();
                dialog.DialogResult = true;
                dialog.Close();
            };


            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Children.Add(btnSave);
            Grid.SetRow(btnSave, 5);
            Grid.SetColumnSpan(btnSave, 2);

            dialog.Content = grid;

            return dialog.ShowDialog() == true ? ticket : null;
        }
    }

    public class Ve
    {
        public ObjectId Id { get; set; }
        public string MaVe { get; set; }
        public string MaLoaiVe { get; set; }
        public string MaKhachHang { get; set; }
        public DateTime NgayKichHoat { get; set; }
        public string TinhTrang { get; set; }
    }
}
