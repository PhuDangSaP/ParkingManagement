using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ParkingManagement.Pages
{
    public partial class QuanLyVe : Page
    {
        private List<Ticket> ticketList = new List<Ticket>();

        public QuanLyVe()
        {
            InitializeComponent();
            LoadTickets();
            UpdateTicketGrid();
        }

        // Load danh sách vé mẫu
        private void LoadTickets()
        {
            ticketList.Clear();
            ticketList.Add(new Ticket { MaVe = "VE001", MaLoaiVe = "L01", MaKhachHang = "KH001", NgayKichHoat = DateTime.Now, TinhTrang = "Đang sử dụng" });
            ticketList.Add(new Ticket { MaVe = "VE002", MaLoaiVe = "L02", MaKhachHang = "KH002", NgayKichHoat = DateTime.Now.AddDays(-10), TinhTrang = "Hết hạn" });
            ticketList.Add(new Ticket { MaVe = "VE003", MaLoaiVe = "L03", MaKhachHang = "KH003", NgayKichHoat = DateTime.Now.AddMonths(-1), TinhTrang = "Đang sử dụng" });
            ticketList.Add(new Ticket { MaVe = "VE004", MaLoaiVe = "L01", MaKhachHang = "KH004", NgayKichHoat = DateTime.Now.AddDays(-20), TinhTrang = "Hết hạn" });
        }

        // Cập nhật lại DataGrid từ danh sách vé
        private void UpdateTicketGrid()
        {
            TicketsDataGrid.Items.Clear();
            foreach (var ticket in ticketList)
            {
                TicketsDataGrid.Items.Add(ticket);
            }
        }

        // Tìm kiếm vé theo mã
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchBox.Text.ToLower();

            // Lọc danh sách vé dựa trên tìm kiếm
            var filteredTickets = ticketList
                .Where(ticket => ticket.MaVe.ToLower().Contains(searchQuery) ||
                                 ticket.MaLoaiVe.ToLower().Contains(searchQuery) ||
                                 ticket.MaKhachHang.ToLower().Contains(searchQuery))
                .ToList();

            // Cập nhật lại DataGrid
            TicketsDataGrid.Items.Clear();
            foreach (var ticket in filteredTickets)
            {
                TicketsDataGrid.Items.Add(ticket);
            }
        }


        private void AddTicket()
        {
            var newTicket = new Ticket
            {
                MaVe = "VE005",
                MaLoaiVe = "L02",
                MaKhachHang = "KH005",
                NgayKichHoat = DateTime.Now,
                TinhTrang = "Đang sử dụng"
            };

            ticketList.Add(newTicket); 
            UpdateTicketGrid();        
        }
    }

    public class Ticket
    {
        public string MaVe { get; set; }
        public string MaLoaiVe { get; set; }
        public string MaKhachHang { get; set; }
        public DateTime NgayKichHoat { get; set; }
        public string TinhTrang { get; set; }
    }
}
