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

        public void LoadTaiKhoan()
        {
            TaiKhoanTable.Clear();

            try
            {
                using (SqlConnection conn = ConnectSQL.ConnectDB())
                {
                    conn.Open();
                    string query = "SELECT * FROM TaiKhoan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaiKhoanTable.Rows.Add(
                                reader["TenDangNhap"].ToString(),
                                reader["MatKhau"].ToString(),
                                reader["MaNhanVien"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy vấn dữ liệu tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        public DataRow LayTaiKhoanTheoMaNhanVien(string maNhanVien)
        {
            foreach (DataRow row in TaiKhoanTable.Rows)
            {
                if (row["MaNhanVien"].ToString() == maNhanVien)
                {
                    return row;
                }
            }
            return null;
        }
    }
}
