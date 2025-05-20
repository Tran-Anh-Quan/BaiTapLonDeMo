using BaiTapLonDeMo.Connect;
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
    public partial class frmQuanLyNhanVien : Form
    {
        private NhanVienVM nhanVienVM;
        private DataTable dtNhanVien;
        DataTable nhanVienTable;
        SqlDataAdapter adapter;
        SqlCommandBuilder builder;


        public frmQuanLyNhanVien()
        {
            InitializeComponent();
            nhanVienVM = new NhanVienVM();
            dtNhanVien = nhanVienVM.LayTatCaNhanVien();
            dgvNhanVien.DataSource = dtNhanVien;
        }

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Nhập mã nhân viên...")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black;
            }
        }
        
        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Nhập mã nhân viên...";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }
        void LoadNhanVien()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QLHD;Integrated Security=True";
            string query = "SELECT * FROM NhanVien";

            SqlConnection conn = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(query, conn);
            builder = new SqlCommandBuilder(adapter);

            nhanVienTable = new DataTable();
            adapter.Fill(nhanVienTable);

            dgvNhanVien.DataSource = nhanVienTable;
        }
        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            dgvNhanVien.AllowUserToDeleteRows = true;
            dgvNhanVien.UserDeletedRow += dgvNhanVien_UserDeletedRow;
            LoadNhanVien();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string luaChon = cboSapXep.SelectedItem?.ToString();
            DataView dv = new DataView(dtNhanVien);
            switch (luaChon)
            {
                case "Lương tăng dần":
                    dv.Sort = "Luong ASC";
                    dv.RowFilter = ""; 
                    break;
                case "Lương giảm dần":
                    dv.Sort = "Luong DESC";
                    dv.RowFilter = "";
                    break;
                case "Nam":
                    dv.RowFilter = "GioiTinh = 'Nam'";
                    dv.Sort = "";
                    break;
                case "Nữ":
                    dv.RowFilter = "GioiTinh = 'Nữ'";
                    dv.Sort = "";
                    break;
                default:
                    dv.RowFilter = "";
                    dv.Sort = "";
                    break;
            }
            dgvNhanVien.DataSource = dv;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvNhanVien.Rows[e.RowIndex].Selected = true;

                var maNhanVienValue = dgvNhanVien.Rows[e.RowIndex].Cells["MaNhanVien"].Value;

                if (maNhanVienValue != null && !string.IsNullOrEmpty(maNhanVienValue.ToString()))
                {
                    string maNhanVien = maNhanVienValue.ToString();
                }
                else
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ.");
                }
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
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


            DataRow nv = nhanVienVM.LayNhanVienTheoMa(maNhanVien);

            if (nv == null)
            {
                stsThongBaoNhanVien.Text = $"❌ Không tìm thấy nhân viên với mã: {maNhanVien}";
                return;
            }

            stsThongBaoNhanVien.Text = $"✅ Đã tìm thấy nhân viên với mã: {maNhanVien}";

   
            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.Cells["MaNhanVien"].Value != null &&
                    row.Cells["MaNhanVien"].Value.ToString().Equals(maNhanVien, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    dgvNhanVien.CurrentCell = row.Cells[0]; // Chọn ô đầu tiên
                    dgvNhanVien.FirstDisplayedScrollingRowIndex = row.Index; // Tự động scroll tới dòng đó
                    break;
                }
            }

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvNhanVien_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                adapter.Update(nhanVienTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi dữ liệu: " + ex.Message);
            }
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            try
            {
                dgvNhanVien.EndEdit();
                adapter.Update(nhanVienTable);
                MessageBox.Show("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi dữ liệu: " + ex.Message);
            }
        }

        //private void dgvNhanVien_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa dòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //    if (result == DialogResult.No)
        //    {
        //        e.Cancel = true;
        //    }
        //}

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa các dòng đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            foreach (DataGridViewRow row in dgvNhanVien.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    DataRowView drv = row.DataBoundItem as DataRowView;
                    if (drv != null)
                    {
                        drv.Row.Delete(); // đánh dấu là đã xóa
                    }
                }
            }

            try
            {
                adapter.Update(nhanVienTable); // đồng bộ với database
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvNhanVien_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                adapter.Update(nhanVienTable);
                MessageBox.Show("Xóa và cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sau khi xóa: " + ex.Message);
            }
        }
    }
}
