using BaiTapLonDeMo.Models;
using BaiTapLonDeMo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLonDeMo.View
{
    public partial class frmDangNhap : Form
    {
        private TaiKhoanVM TKVM = new TaiKhoanVM();
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool ktDangNhap = TKVM.DangNhapTaiKhoan(tenDangNhap, matKhau);
            if (ktDangNhap)
            {
                stsFooter.Text = "Đăng nhập thành công!";
                await Task.Delay(1000);
                this.Hide();
                frmMain mainForm = new frmMain();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                stsFooter.Text = " Đăng nhập thất bại! Tên đăng nhập hoặc mật khẩu không đúng!";
                txtUsername.Focus();
            }

        }
    }
}
