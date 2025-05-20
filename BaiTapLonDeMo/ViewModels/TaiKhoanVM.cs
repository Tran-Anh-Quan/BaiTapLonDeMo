using BaiTapLonDeMo.Connect;
using BaiTapLonDeMo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLonDeMo.ViewModels
{
    public class TaiKhoanVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public DataTable TaiKhoanTable { get; private set; }

        public TaiKhoanVM()
        {
            TaiKhoanTable = new DataTable();
            TaiKhoanTable.Columns.Add("TenDangNhap", typeof(string));
            TaiKhoanTable.Columns.Add("MatKhau", typeof(string));
            TaiKhoanTable.Columns.Add("MaNhanVien", typeof(string));
        }

        

        public bool DangNhapTaiKhoan(string tenDangNhap, string matKhau)
        {
            try
            {
                using (SqlConnection conn = ConnectSQL.ConnectDB())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối hoặc truy vấn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public DataTable LayTatCatkNhanVien()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ConnectSQL.ConnectDB())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM TaiKhoan";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message);
                }
            }

            return dt;
        }
        public DataRow LayTaiKhoanTheoMa(string maNhanVien)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ConnectSQL.ConnectDB())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM TaiKhoan WHERE MaNhanVien = @MaNhanVien";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải Tài Khoản: " + ex.Message);
                }
            }

            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

    }
}
