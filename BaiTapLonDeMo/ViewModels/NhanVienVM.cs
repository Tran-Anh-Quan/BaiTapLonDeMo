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
    public class NhanVienVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public DataTable LayTatCaNhanVien()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ConnectSQL.ConnectDB())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM NhanVien";
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

        public DataRow LayNhanVienTheoMa(string maNhanVien)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ConnectSQL.ConnectDB())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải nhân viên: " + ex.Message);
                }
            }

            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }
    }
}


