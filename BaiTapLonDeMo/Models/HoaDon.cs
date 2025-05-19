using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapLonDeMo.Models
{
    public class HoaDon
    {
        public string SoHD { get; set; }
        public DateTime NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public string HoTen { get; set; }
        public decimal TongTien { get; set; }
        public decimal Thue { get; set; }
        public decimal ChietKhau { get; set; }
        public decimal ThanhTien { get; set; }
        public string MaKH {get; set;}
    }
}
