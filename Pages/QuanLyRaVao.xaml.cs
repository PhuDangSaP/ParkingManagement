using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingManagement.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyRaVao.xaml
    /// </summary>
    public partial class QuanLyRaVao : Page
    {
        private List<ViTriDo> listViTriDo = new List<ViTriDo>();
        private List<Xe> listXe = new List<Xe>();

        public QuanLyRaVao()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            listViTriDo = GetListViTriDo();
        }

        public string XeVao(string bienSo, string loaiXe)
        {
            var viTriTrong = listViTriDo.FirstOrDefault(v => v.trangThai == "Trống");
            if (viTriTrong == null)
                return "Không còn chỗ trống.";

            var xe = new Xe
            {
                BienSo = bienSo,
                LoaiXe = loaiXe,
                MaVTD = viTriTrong.maVTD,
                ThoiGianVao = DateTime.Now
            };
            listXe.Add(xe);
            viTriTrong.trangThai = "Đang sử dụng";

            UpdateTrangThaiViTri(viTriTrong.maVTD, "Đang sử dụng");
            SaveXeVao(xe);

            return $"Xe {bienSo} đã vào bãi tại vị trí {viTriTrong.tenVTD}.";
        }

        public string XeRa(string bienSo)
        {
            var xe = listXe.FirstOrDefault(x => x.BienSo == bienSo);
            if (xe == null)
                return "Không tìm thấy xe.";

            var viTriDo = listViTriDo.FirstOrDefault(v => v.maVTD == xe.MaVTD);
            if (viTriDo != null)
            {
                viTriDo.trangThai = "Trống";
                UpdateTrangThaiViTri(viTriDo.maVTD, "Trống");
            }

            xe.ThoiGianRa = DateTime.Now;
            SaveXeRa(xe);
            listXe.Remove(xe);

            return $"Xe {bienSo} đã rời bãi.";
        }

        private List<ViTriDo> GetListViTriDo()
        {
            var list = DatabaseHandler.Instance.GetCollection("ViTriDo").Find(new BsonDocument()).ToList();
            return list.Select(item => new ViTriDo
            {
                maVTD = item["MaVTD"].AsString,
                maBD = item["MaBD"].AsString,
                tenVTD = item["TenVTD"].AsString,
                trangThai = item["TrangThai"].AsString
            }).ToList();
        }

        private void UpdateTrangThaiViTri(string maVTD, string trangThai)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("MaVTD", maVTD);
            var update = Builders<BsonDocument>.Update.Set("TrangThai", trangThai);
            DatabaseHandler.Instance.GetCollection("ViTriDo").UpdateOne(filter, update);
        }

        private void SaveXeVao(Xe xe)
        {
            var document = new BsonDocument
            {
                { "BienSo", xe.BienSo },
                { "LoaiXe", xe.LoaiXe },
                { "MaVTD", xe.MaVTD },
                { "ThoiGianVao", xe.ThoiGianVao }
            };
            DatabaseHandler.Instance.GetCollection("XeVaoRa").InsertOne(document);
        }

        private void SaveXeRa(Xe xe)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("BienSo", xe.BienSo);
            var update = Builders<BsonDocument>.Update.Set("ThoiGianRa", xe.ThoiGianRa);
            DatabaseHandler.Instance.GetCollection("XeVaoRa").UpdateOne(filter, update);
        }
    }
}
