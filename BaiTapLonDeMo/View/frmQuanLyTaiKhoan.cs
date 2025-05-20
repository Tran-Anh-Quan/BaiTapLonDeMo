using BaiTapLonDeMo.Models;
using BaiTapLonDeMo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLonDeMo.View
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        private TaiKhoanVM taiKhoanVM;
        private DataTable dtTaiKhoan;
        SqlDataAdapter adapter;
        SqlCommandBuilder builder;
        public frmQuanLyTaiKhoan()
        {       
            InitializeComponent();
            LoadTaiKhoan();
            taiKhoanVM = new TaiKhoanVM();
        }


        public void LoadTaiKhoan()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QLHD;Integrated Security=True";
            string query = "SELECT * FROM TaiKhoan";

            SqlConnection conn = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(query, conn);
            builder = new SqlCommandBuilder(adapter);

            dtTaiKhoan = new DataTable();
            adapter.Fill(dtTaiKhoan);

            dgvTaiKhoan.DataSource = dtTaiKhoan;
        }

        private void txtTimKiem_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Nhập mã nhân viên...";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Nhập mã nhân viên...")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black;
            }
        }

        private void dgvTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvTaiKhoan.Rows[e.RowIndex].Selected = true;

                var maNhanVienValue = dgvTaiKhoan.Rows[e.RowIndex].Cells["MaNhanVien"].Value;

                if (maNhanVienValue != null && !string.IsNullOrEmpty(maNhanVienValue.ToString()))
                {
                    string maNhanVien = maNhanVienValue.ToString();
                }
                else
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ.");
                }
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            stsThongBaoNhanVien.Text = "";

            string maNhanVien = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(maNhanVien) || maNhanVien == "Nhập mã nhân viên...")
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên cần tìm kiếm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DataRow nv = taiKhoanVM.LayTaiKhoanTheoMa(maNhanVien);

            if (nv == null)
            {
                stsThongBaoNhanVien.Text = $"❌ Không tìm thấy nhân viên với mã: {maNhanVien}";
                return;
            }

            stsThongBaoNhanVien.Text = $"✅ Đã tìm thấy nhân viên với mã: {maNhanVien}";


            foreach (DataGridViewRow row in dgvTaiKhoan.Rows)
            {
                if (row.Cells["MaNhanVien"].Value != null &&
                    row.Cells["MaNhanVien"].Value.ToString().Equals(maNhanVien, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    dgvTaiKhoan.CurrentCell = row.Cells[0];
                    dgvTaiKhoan.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            try
            {
                dgvTaiKhoan.EndEdit();
                adapter.Update(dtTaiKhoan);
                MessageBox.Show("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi dữ liệu: " + ex.Message);
            }
        }
        private void dgvTaiKhoan_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa dòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void dgvTaiKhoan_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                adapter.Update(dtTaiKhoan);
                MessageBox.Show("Xóa và cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sau khi xóa: " + ex.Message);
            }
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            dgvTaiKhoan.UserDeletedRow += dgvTaiKhoan_UserDeletedRow;
            dgvTaiKhoan.RowValidated += dgvTaiKhoan_RowValidated;


        }

        private void dgvTaiKhoan_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgvTaiKhoan.EndEdit();
                int rowsUpdated = adapter.Update(dtTaiKhoan);
                if (rowsUpdated > 0)
                {
                    stsThongBaoNhanVien.Text = $"✅ Đã cập nhật thành công {rowsUpdated} dòng.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi ghi dữ liệu: " + ex.Message);
            }
        }
    }
}
