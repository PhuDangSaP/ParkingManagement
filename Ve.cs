using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ParkingManagement
{
    public class Ve
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("MaVe")]
        public string MaVe { get; set; }

        [BsonElement("MaLoaiVe")]
        public string MaLoaiVe { get; set; }

        [BsonElement("MaKhachHang")]
        public string MaKhachHang { get; set; }

        [BsonElement("NgayKichHoat")]
        public DateTime NgayKichHoat { get; set; }

        [BsonElement("TinhTrang")]
        public string TinhTrang { get; set; }
    }
}
