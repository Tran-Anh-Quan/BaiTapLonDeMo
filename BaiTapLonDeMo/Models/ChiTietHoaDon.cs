using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapLonDeMo.Models
{
    public class ChiTietHoaDon
    {
        public string SoHD { get; set; }
        public string TenHang { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal Tien => SoLuong * GiaBan;
        public string MaHang { get; set; }
    }
}
