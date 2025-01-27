﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PBL.BLL;
using PBL.DAL;
using PBL.DTO;

namespace PBL
{
    public partial class fMenuQuanLy : Form
    {
        public delegate void Delegate_fHome();
        public Delegate_fHome Send { get; set; }
        private string NhanVienID;
        public fMenuQuanLy(string nhanvienid)
        {
            InitializeComponent();
            NhanVienID = nhanvienid;
            GUIPhong();
            GUILoaiPhong();
            GUILoaiVatDung();
            GUIDichVu();
            GUIBillDV();
            GuiKhachHang();
            LoadMenu();
        }

        #region Load

        private void LoadMenu()
        {
            if (BLL_QLNV.Instance.GetQuyenHanByNhanVienID(NhanVienID) == 1)
            {
                tabcontrol.TabPages.Remove(tpPhong);
                tabcontrol.TabPages.Remove(tpLoaiDV);
                tabcontrol.TabPages.Remove(tpLoaiPhong);
                tabcontrol.TabPages.Remove(tpLoaiVatDung);
            }
            else if (BLL_QLNV.Instance.GetQuyenHanByNhanVienID(NhanVienID) == 4)
            {
                tabcontrol.TabPages.Remove(tpKhachHang);
                tabcontrol.TabPages.Remove(tpHoaDonDV);
                tabcontrol.TabPages.Remove(tpLoaiDV);
            }
        }

        #endregion

        #region Quản lý phòng
        private void GUIPhong()
        {
            foreach(LOAIPHONG i in BLL_QLLP.Instance.GetListLoaiPhong(null))
            {
                cbTenLoaiPhong.Items.Add(new CBBItem
                {
                    Text = i.TenLoaiPhong,
                    Value = i.LoaiPhongID
                });
            }
            cbSortPhong.Items.AddRange(new string[]
            {
                "Tên phòng",
                "Tên loại phòng",
                "Trạng thái"
            });
            ShowDGVPhong(null);
            rbtAvailable.Checked = true;
        }
        private void ShowDGVPhong(string s = null)
        {
            dgvPhong.DataSource = BLL_QLP.Instance.GetListPhong_View(BLL_QLP.Instance.GetListPhong(s));
            dgvPhong.Columns["PhongID"].HeaderText = "Tên phòng";
            dgvPhong.Columns["TenLoaiPhong"].HeaderText = "Loại phòng";
            dgvPhong.Columns["TrangThai"].HeaderText = "Trạng thái";
        }
        private void RefreshGUIPhong()
        {
            cbTenLoaiPhong.Items.Clear();
            cbTenLoaiPhong.ResetText();
            foreach (LOAIPHONG i in BLL_QLLP.Instance.GetListLoaiPhong(null))
            {
                cbTenLoaiPhong.Items.Add(new CBBItem
                {
                    Text = i.TenLoaiPhong,
                    Value = i.LoaiPhongID
                });
            }
            ShowDGVPhong();
        }
        private void btnThemPh_Click(object sender, EventArgs e)
        {
            if (BLL_QLP.Instance.FindPhong(txbMaPhong.Text.Trim()) == null)
            {
                if (string.IsNullOrEmpty(txbMaPhong.Text.Trim()))
                {
                    MessageBox.Show("Mã phòng không được để trống !");
                }
                else if (cbTenLoaiPhong.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn loại phòng !");
                }
                else
                {
                    try
                    {
                        PHONG p = new PHONG
                        {
                            PhongID = txbMaPhong.Text.Trim(),
                            LoaiPhongID = ((CBBItem)cbTenLoaiPhong.SelectedItem).Value,
                            TrangThai = rbtAvailable.Checked
                        };
                        BLL_QLP.Instance.AddPhong(p);
                        ShowDGVPhong(null);
                    }
                    catch
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ và đúng kiểu dữ liệu của thông tin !");
                    }
                }       
            }
            else
            {
                MessageBox.Show("Mã phòng đã tồn tại!");
            }
            
        }

        private void btnSuaPh_Click(object sender, EventArgs e)
        {
            if (cbTenLoaiPhong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại phòng !");
            }
            else if (BLL_QLP.Instance.FindPhong(txbMaPhong.Text.Trim()) != null)
            {
                try
                {
                    PHONG p = new PHONG
                    {
                        PhongID = txbMaPhong.Text.Trim(),
                        LoaiPhongID = ((CBBItem)cbTenLoaiPhong.SelectedItem).Value,
                        TrangThai = rbtAvailable.Checked
                    };
                    BLL_QLP.Instance.UpdatePhong(p);
                    ShowDGVPhong(null);
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ và đúng kiểu dữ liệu của thông tin !");
                }
            }
            else
            {
                MessageBox.Show("Mã phòng không tồn tại!");
            }
            
        }
        private void btnXoaPh_Click(object sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count > 0)
            {
                if(BLL_QLP.Instance.DeletePhong(GetListPhongID()) == true)
                {
                    ShowDGVPhong(null);
                }
                else
                {
                    MessageBox.Show("Không thể xoá phòng này!\n" +
                        "Có người đang dùng hoặc có Book liên quan đến phòng này !");
                }
            }
            else
            {
                MessageBox.Show("Chọn ít nhất một dòng để xóa !");
            }
        }

        private void btnResetPh_Click(object sender, EventArgs e)
        {
            txbMaPhong.Clear();
            cbTenLoaiPhong.SelectedIndex = -1;
        }

        private void btnSearchP_Click(object sender, EventArgs e)
        {
            ShowDGVPhong(txbSeachP.Text);
        }

        private void btnResetSp_Click(object sender, EventArgs e)
        {
            txbSeachP.Clear();
            ShowDGVPhong(null);
        }
        private List <string> GetListPhongID()
        {
            List<string> data = new List<string>();
            foreach(DataGridViewRow r in dgvPhong.SelectedRows)
            {
                data.Add(r.Cells["PhongID"].Value.ToString());
            }
            return data;
        }
        private int cbTenLoaiPhongIndexOf(string s)
        {
            for(int i = 0; i < cbTenLoaiPhong.Items.Count; i++)
            {
                if ((((CBBItem)cbTenLoaiPhong.Items[i]).Value) == s)
                {
                    return i;
                }

            }
            return -1;
        }
        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            PHONG p = BLL_QLP.Instance.FindPhong(dgvPhong.SelectedRows[0].Cells["PhongID"].Value.ToString());
            txbMaPhong.Text = p.PhongID;
            cbTenLoaiPhong.SelectedIndex = cbTenLoaiPhongIndexOf(p.LoaiPhongID);
            rbtAvailable.Checked = p.TrangThai;
            rbtNotAvailable.Checked = !p.TrangThai;
        }
        private List<string> GetAllPhong()
        {
            List<string> data = new List<string>();
            foreach(DataGridViewRow r in dgvPhong.Rows)
            {
                data.Add(r.Cells["PhongID"].Value.ToString());
            }
            return data;
        }
        private void cbSortPhong_DropDownClosed(object sender, EventArgs e)
        {
            if(cbSortPhong.SelectedIndex != -1)
            {
                dgvPhong.DataSource = BLL_QLP.Instance.GetListPhong_View(BLL_QLP.Instance.Sort(cbSortPhong.SelectedItem.ToString(), GetAllPhong()));
            }
        }

        private void btnSortPhong_Click(object sender, EventArgs e)
        {
            if (cbSortPhong.SelectedIndex != -1)
            {
                dgvPhong.DataSource = BLL_QLP.Instance.GetListPhong_View(BLL_QLP.Instance.Sort(cbSortPhong.SelectedItem.ToString(), GetAllPhong()));
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn mục để sắp xếp !");
            }
        }
        private void txbSeachP_TextChanged(object sender, EventArgs e)
        {
            btnSearchP.PerformClick();
        }
        #endregion

        #region Quản lý loại phòng
        private void GUILoaiPhong()
        {
            cbSortLP.Items.AddRange(new string[] { "Tên loại phòng", "Số người", "Giá" });
            ShowDGVLoaiPhong(null);
        }
        private List <string> GetListLoaiPhongID()
        {
            List<string> data = new List<string>();
            foreach(DataGridViewRow r in dgvLoaiPhong.SelectedRows)
            {
                data.Add(r.Cells["LoaiPhongID"].Value.ToString());
            }
            return data;
        }
        private void ShowDGVLoaiPhong(string s = null)
        {
            dgvLoaiPhong.DataSource = BLL_QLLP.Instance.GetListLoaiPhong(s);
            dgvLoaiPhong.Columns["LoaiPhongID"].Visible = false;
            dgvLoaiPhong.Columns["PHONGs"].Visible = false;
            dgvLoaiPhong.Columns["Gia"].HeaderText = "Giá";
            dgvLoaiPhong.Columns["SoNguoi"].HeaderText = "Số người";
            dgvLoaiPhong.Columns["TenLoaiPhong"].HeaderText = "Tên loại phòng";

        }
        private void btnThemLP_Click(object sender, EventArgs e)
        {
            if (BLL_QLLP.Instance.FindLoaiPhong(txbTenLoaiPhong.Text.Trim()) == null)
            {
                try
                {
                    LOAIPHONG lp = new LOAIPHONG
                    {
                        TenLoaiPhong = txbTenLoaiPhong.Text.Trim(),
                        Gia = Convert.ToDecimal(txbGiaLP.Text.Trim()),
                        SoNguoi = Convert.ToInt32(nUDSoNguoi.Value),
                    };
                    BLL_QLLP.Instance.AddLoaiPhong(lp);
                    ShowDGVLoaiPhong(null);
                    RefreshGUIPhong();
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin !");
                }
            }
            else
            {
                MessageBox.Show("Tên loại phòng đã tồn tại !");
            }
            
        }

        private void btnSuaLP_Click(object sender, EventArgs e)
        {
            if (dgvLoaiPhong.SelectedRows.Count == 1)
            {
                if (BLL_QLLP.Instance.FindLoaiPhong(txbTenLoaiPhong.Text.Trim()) != null)
                {
                    LOAIPHONG lp = new LOAIPHONG
                    {
                        LoaiPhongID = dgvLoaiPhong.SelectedRows[0].Cells["LoaiPhongID"].Value.ToString(),
                        TenLoaiPhong = txbTenLoaiPhong.Text.Trim(),
                        Gia = Convert.ToDecimal(txbGiaLP.Text.Trim()),
                        SoNguoi = Convert.ToInt32(nUDSoNguoi.Value)
                    };
                    BLL_QLLP.Instance.UpdateLoaiPhong(lp);
                    ShowDGVLoaiPhong(null);
                    RefreshGUIPhong();
                }
                else
                {
                    MessageBox.Show("Tên loại phòng này không tồn tại !");
                }
            }
            else
            {
                MessageBox.Show("Chọn một dòng loại phòng duy nhất để sửa !");
            }
            
        }

        private void btnXoaLP_Click(object sender, EventArgs e)
        {
            if (dgvLoaiPhong.SelectedRows.Count > 0)
            {

                if(BLL_QLLP.Instance.DeleteLoaiPhong(GetListLoaiPhongID()) == true)
                {
                    ShowDGVLoaiPhong();
                    RefreshGUIPhong();
                }
                else
                {
                    MessageBox.Show("Không thể xoá loại phòng này !\n" +
                        " Có phòng đang dùng loại phòng này !\n" +
                        " Vui lòng sửa loại phòng sau đó thử lại !");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dòng để xóa!");
            }
        }

        private void btnResetLP_Click(object sender, EventArgs e)
        {
            txbGiaLP.Clear();
            txbTenLoaiPhong.Clear();
            nUDSoNguoi.Value = 0;
        }

        private void btnSearchLP_Click(object sender, EventArgs e)
        {
            ShowDGVLoaiPhong(txbSeachLp.Text.Trim());
        }

        private void btnResetSLp_Click(object sender, EventArgs e)
        {
            txbSeachLp.Clear();
            ShowDGVLoaiPhong(null);
        }

        private void dgvLoaiPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            LOAIPHONG lp = BLL_QLLP.Instance.FindLoaiPhong(dgvLoaiPhong.SelectedRows[0].Cells["TenLoaiPhong"].Value.ToString());
            txbGiaLP.Text = lp.Gia.ToString();
            txbTenLoaiPhong.Text = lp.TenLoaiPhong;
            nUDSoNguoi.Value = lp.SoNguoi;
        }
        private List<string> GetAllDGVTenLoaiPhong()
        {
            List<string> data = new List<string>();
            foreach(DataGridViewRow r in dgvLoaiPhong.Rows)
            {
                data.Add(r.Cells["TenLoaiPhong"].Value.ToString());
            }
            return data;
        }
        private void cbSortLP_DropDownClosed(object sender, EventArgs e)
        {
            if (cbSortLP.SelectedIndex != -1)
            {
                dgvLoaiPhong.DataSource = BLL_QLLP.Instance.Sort(cbSortLP.SelectedItem.ToString(), GetAllDGVTenLoaiPhong());
                dgvLoaiPhong.Columns["LoaiPhongID"].Visible = false;
                dgvLoaiPhong.Columns["PHONGs"].Visible = false;
            }
        }
        private void btnSortLP_Click(object sender, EventArgs e)
        {
            if (cbSortLP.SelectedIndex != -1)
            {
                dgvLoaiPhong.DataSource = BLL_QLLP.Instance.Sort(cbSortLP.SelectedItem.ToString(), GetAllDGVTenLoaiPhong());
                dgvLoaiPhong.Columns["LoaiPhongID"].Visible = false;
                dgvLoaiPhong.Columns["PHONGs"].Visible = false;
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn mục để sắp xếp !");
            }
        }
        private void txbSeachLp_TextChanged(object sender, EventArgs e)
        {
            btnSearchLP.PerformClick();
        }
        #endregion

        #region Quản lý loại vật dụng

        private void GUILoaiVatDung()
        {
            cbSortLoaiVD.Items.AddRange(new string[] { "Tên vật dụng", "Đơn giá" });
            ShowDGVLoaiVatDung(null);
        }
        private void ShowDGVLoaiVatDung(string s)
        {
            dgvLoaiVatDung.DataSource = BLL_QLVD.Instance.GetListLoaiVatDung(s);
            dgvLoaiVatDung.Columns["VatDungID"].Visible = false;
            dgvLoaiVatDung.Columns["VATDUNGPHONGs"].Visible = false;
            dgvLoaiVatDung.Columns["TenVatDung"].HeaderText = "Tên vật dụng";
            dgvLoaiVatDung.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvLoaiVatDung.Columns["ThietBiCoDinh"].HeaderText = "Thiết bị cố định";
        }
        private List<string> GetListVatDungID()
        {
            List<string> data = new List<string>();
            foreach(DataGridViewRow r in dgvLoaiVatDung.SelectedRows)
            {
                data.Add(r.Cells["VatDungID"].Value.ToString());
            }
            return data;
        }
        private List<string> GetAllDGVTenVatDung()
        {
            List<string> data = new List<string>();
            foreach(DataGridViewRow r in dgvLoaiVatDung.Rows)
            {
                data.Add(r.Cells["TenVatDung"].Value.ToString());
            }
            return data;
        }
        private void btnThemVT_Click(object sender, EventArgs e)
        {
            if (BLL_QLVD.Instance.FindLoaiVatDung(txbTenVt.Text.Trim()) == null)
            {
                try
                {
                    LOAIVATDUNG lvd = new LOAIVATDUNG
                    {
                        TenVatDung = txbTenVt.Text.Trim(),
                        ThietBiCoDinh = checkBoxTBCoDinh.Checked,
                        DonGia = Convert.ToDecimal(txbDonGiaVt.Text.Trim())
                    };
                    BLL_QLVD.Instance.AddLoaiVatDung(lvd);
                    ShowDGVLoaiVatDung(null);
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin và đúng kiểu dữ liệu !");
                }
            }
            else
            {
                MessageBox.Show("Tên vật dụng đã tồn tại !");
            }
        }

        private void btnSuaVT_Click(object sender, EventArgs e)
        {
            if(dgvLoaiVatDung.SelectedRows.Count == 1)
            {
                if (BLL_QLVD.Instance.FindLoaiVatDung(txbTenVt.Text.Trim()) != null)
                {
                    try
                    {
                        LOAIVATDUNG lvd = new LOAIVATDUNG
                        {
                            VatDungID = dgvLoaiVatDung.SelectedRows[0].Cells["VatDungID"].Value.ToString(),
                            TenVatDung = txbTenVt.Text.Trim(),
                            ThietBiCoDinh = checkBoxTBCoDinh.Checked,
                            DonGia = Convert.ToDecimal(txbDonGiaVt.Text.Trim())
                        };
                        BLL_QLVD.Instance.UpdateLoaiVatDung(lvd);
                        ShowDGVLoaiVatDung(null);
                    }
                    catch
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin và đúng kiểu dữ liệu !");
                    }
                }
                else
                {
                    MessageBox.Show("Tên vật dụng không tồn tại !");
                }
            }
            else
            {
                MessageBox.Show("Chọn duy nhất một dòng để sửa !");
            }
            
        }

        private void btnXoaVT_Click(object sender, EventArgs e)
        {
            if (BLL_QLVD.Instance.DeleteLoaiVatDung(GetListVatDungID()) == true) 
            {
                ShowDGVLoaiVatDung(null);
            }
            else
            {
                MessageBox.Show("Không thể xoá loại vật tư này,\n" +
                    " loại vật tư này đang được dùng trong phần quản lý vật tư phòng,\n" +
                    " vui lòng xoá dữ liệu liên quan bên phần quản lý vật tư phòng trước !");
            }
        }

        private void btnResetVT_Click(object sender, EventArgs e)
        {
            txbTenVt.Clear();
            txbDonGiaVt.Clear();
            checkBoxTBCoDinh.Checked = false;
        }

        private void BtnSearchVT_Click(object sender, EventArgs e)
        {
            ShowDGVLoaiVatDung(txbSeachVD.Text.Trim());        }

        private void btnResetSVT_Click(object sender, EventArgs e)
        {
            txbSeachVD.Clear();
            ShowDGVLoaiVatDung(null);
        }


        private void dgvVatTu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            LOAIVATDUNG lvd = BLL_QLVD.Instance.FindLoaiVatDung(dgvLoaiVatDung.SelectedRows[0].Cells["TenVatDung"].Value.ToString());
            txbTenVt.Text = lvd.TenVatDung;
            checkBoxTBCoDinh.Checked = (bool)lvd.ThietBiCoDinh;
            txbDonGiaVt.Text = lvd.DonGia.ToString();
        }
        private void cbSortLoaiVD_DropDownClosed(object sender, EventArgs e)
        {
            if(cbSortLoaiVD.SelectedIndex != -1)
            {
                dgvLoaiVatDung.DataSource = BLL_QLVD.Instance.Sort(cbSortLoaiVD.SelectedItem.ToString(), GetAllDGVTenVatDung());
                dgvLoaiVatDung.Columns["VatDungID"].Visible = false;
                dgvLoaiVatDung.Columns["VATDUNGPHONGs"].Visible = false;
            }
        }
        private void btnSortLoaiVD_Click(object sender, EventArgs e)
        {
            if (cbSortLoaiVD.SelectedIndex != -1)
            {
                dgvLoaiVatDung.DataSource = BLL_QLVD.Instance.Sort(cbSortLoaiVD.SelectedItem.ToString(), GetAllDGVTenVatDung());
                dgvLoaiVatDung.Columns["VatDungID"].Visible = false;
                dgvLoaiVatDung.Columns["VATDUNGPHONGs"].Visible = false;
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn mục để sắp xếp !");
            }
        }
        private void txbSeachVD_TextChanged(object sender, EventArgs e)
        {
            BtnSearchVT.PerformClick();
        }
        #endregion

        #region Quản lý dịch vụ
        private void GUIDichVu()
        {
            ShowDgvDV();
            cbSortDV.SelectedIndex = 0;
        }

        private void ShowDgvDV()
        {
            dgvDichVu.DataSource = BLL_QLDV.Instance.GetAllDichVu();
            dgvDichVu.Columns["DichVuID"].Visible = false;
            dgvDichVu.Columns["HOADON_DUNG_DICHVU"].Visible = false;
            dgvDichVu.Columns["TenDichVu"].HeaderText = "Tên dịch vụ";
            dgvDichVu.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvDichVu.Columns["GioMo"].HeaderText = "Giờ mở";
            dgvDichVu.Columns["GioDong"].HeaderText = "Giờ đóng";
        }

        private void RefreshDV()
        {
            txbTenDV.Clear();
            txbGiaDV.Clear();
            txbOpenDV.Clear();
            txbCloseDV.Clear();
            ShowDgvDV();
        }

        private void btnThemDV_Click(object sender, EventArgs e)
        {
            if (txbTenDV.TextLength != 0 && txbGiaDV.TextLength != 0)
            {
                try
                {
                    LOAIDICHVU dv = new LOAIDICHVU
                    {
                        TenDichVu = txbTenDV.Text,
                        DonGia = Convert.ToDecimal(txbGiaDV.Text),
                        GioMo = TimeSpan.Parse(txbOpenDV.Text),
                        GioDong = TimeSpan.Parse(txbCloseDV.Text)
                    };

                    if (BLL_QLDV.Instance.AddDichVu(dv))
                    {
                        MessageBox.Show("Thêm dịch vụ thành công!");
                        RefreshDV();
                        RefreshGUIBillDichVu();
                        Send();
                    }
                    else
                    {
                        MessageBox.Show("Thêm dịch vụ không thành công! Vui lòng kiểm tra lại thông tin");
                    }
                }
                catch
                {
                    MessageBox.Show("Vui lòng kiểm tra thông tin nhập!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên và giá dịch vụ!");
            }
        }

        private void btnSuaDV_Click(object sender, EventArgs e)
        {
            try
            {
                if (txbTenDV.TextLength != 0 && txbGiaDV.TextLength != 0)
                {
                    LOAIDICHVU dv = new LOAIDICHVU
                    {
                        DichVuID = dgvDichVu.SelectedRows[0].Cells["DichVuID"].Value.ToString(),
                        TenDichVu = txbTenDV.Text,
                        DonGia = Convert.ToDecimal(txbGiaDV.Text),
                        GioMo = TimeSpan.Parse(txbOpenDV.Text),
                        GioDong = TimeSpan.Parse(txbCloseDV.Text)
                    };
                    if (BLL_QLDV.Instance.UpdateDichVu(dv))
                    {
                        MessageBox.Show("Cập nhật dịch vụ thành công!");
                        RefreshDV();
                        RefreshGUIBillDichVu();
                        Send();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật dịch vụ không thành công! Vui lòng kiểm tra lại thông tin");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin dịch vụ!");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần cập nhật!");
            }
        }

        private void btnXoaDV_Click(object sender, EventArgs e)
        {
            try
            {
                string dvid = dgvDichVu.SelectedRows[0].Cells["DichVuID"].Value.ToString();
                if (BLL_QLDV.Instance.DeleteDichVu(dvid))
                {
                    MessageBox.Show("Xóa dịch vụ thành công!");
                    RefreshDV();
                    RefreshGUIBillDichVu();
                    Send();
                }
                else
                {
                    MessageBox.Show("Dịch vụ không thể xóa !\n" +
                        "Vui lòng kiểm tra lại !");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
            }
        }

        private void btnResetDV_Click(object sender, EventArgs e)
        {
            RefreshDV();
        }

        private void btnSearchDv_Click(object sender, EventArgs e)
        {
            dgvDichVu.DataSource = BLL_QLDV.Instance.GetListDichVu(txbSeachDv.Text);
        }

        private void btnResetSDv_Click(object sender, EventArgs e)
        {
            txbSeachDv.Clear();
        }

        private void btnSortDV_Click(object sender, EventArgs e)
        {
            if (cbSortDV.SelectedIndex >= 0)
            {
                List<string> LdvID = new List<string>();
                for (int i = 0; i < dgvDichVu.Rows.Count; i++)
                {
                    LdvID.Add(dgvDichVu.Rows[i].Cells["DichVuID"].Value.ToString());
                }
                dgvDichVu.DataSource = BLL_QLDV.Instance.GetListDichVuSorted(LdvID, cbSortDV.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn kiểu sắp xếp!");
            }
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            txbTenDV.Text = dgvDichVu.SelectedRows[0].Cells["TenDichVu"].Value.ToString();
            txbGiaDV.Text = dgvDichVu.SelectedRows[0].Cells["DonGia"].Value.ToString();
            txbOpenDV.Text = dgvDichVu.SelectedRows[0].Cells["GioMo"].Value.ToString();
            txbCloseDV.Text = dgvDichVu.SelectedRows[0].Cells["GioDong"].Value.ToString();
        }


        private void dgvDichVu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvDichVu.ClearSelection();
        }
        #endregion

        #region Quản lý Bill dịch vụ
        private void GUIBillDV()
        {
            foreach(LOAIDICHVU i in BLL_QLDV.Instance.GetAllDichVu())
            {
                cbTenDV.Items.Add(new CBBItem { Text = i.TenDichVu, Value = i.DichVuID });
            }
            cbSortBillDV.Items.AddRange(new string[]
            {
                "Mã Book",
                "Tên nhân viên",
                "Tên dịch vụ",
                "Ngày",
                "Số lượng",
                "Thành tiền"
            });
            cbTenDV.SelectedIndex = 0;
            ShowDGVBillDV();
        }
        private void RefreshGUIBillDichVu()
        {
            cbTenDV.Items.Clear();
            cbTenDV.ResetText();
            foreach (LOAIDICHVU i in BLL_QLDV.Instance.GetAllDichVu())
            {
                cbTenDV.Items.Add(new CBBItem { Text = i.TenDichVu, Value = i.DichVuID });
            }
            ShowDGVBillDV();
        }
        private void ShowDGVBillDV(string s = null)
        {
            dgvBillDV.DataSource = BLL_QLBillDV.Instance.GetListBillDV_View(BLL_QLBillDV.Instance.GetListBillDV(s));
            dgvBillDV.Columns["ID"].Visible = false;
            dgvBillDV.Columns["BookID"].HeaderText = "Mã Book";
            dgvBillDV.Columns["TenNhanVien"].HeaderText = "Tên nhân viên";
            dgvBillDV.Columns["TenDichVu"].HeaderText = "Tên dịch vụ";
            dgvBillDV.Columns["Ngay"].HeaderText = "Ngày";
            dgvBillDV.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvBillDV.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }
        private void btnThemBill_Click(object sender, EventArgs e)
        {
            int billid = -1;
            Int32.TryParse(txbMaBill.Text, out billid);
            if (BLL_QLBillDV.Instance.FindBillDV(billid) != null)
            {
                MessageBox.Show("Mã hoá đơn dịch vụ đã tồn tại !");
            }
            else if (BLL_QLBOOK.Instance.Find(txbMaBook.Text.Trim()) == null)
            {
                MessageBox.Show("Mã Book không tồn tại !");
            }
            else
            {
                try
                {
                    HOADON_DUNG_DICHVU p = new HOADON_DUNG_DICHVU
                    {
                        BookID = txbMaBook.Text.Trim(),
                        DichVuID = ((CBBItem)cbTenDV.SelectedItem).Value.Trim(),
                        SoLuong = Convert.ToInt32(nUDSoLuong.Value),
                        Ngay = dtpNgayDat.Value,
                        NhanVienID = this.NhanVienID
                    };
                    BLL_QLBillDV.Instance.AddBillDV(p);
                    ShowDGVBillDV();
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin và kiểu dữ liệu !");
                }
            }          
        }

        private void btnSuaBill_Click(object sender, EventArgs e)
        {
            int billid = -1;
            Int32.TryParse(txbMaBill.Text, out billid);
            if (BLL_QLBillDV.Instance.FindBillDV(billid) == null)
            {
                MessageBox.Show("Mã hoá đơn dịch vụ không tồn tại !");
            }
            else if (BLL_QLBOOK.Instance.Find(txbMaBook.Text.Trim()) == null)
            {
                MessageBox.Show("Mã Book không tồn tại !");
            }
            else
            {
                try
                {
                    HOADON_DUNG_DICHVU p = new HOADON_DUNG_DICHVU
                    {
                        ID = billid,
                        BookID = txbMaBook.Text.Trim(),
                        DichVuID = ((CBBItem)cbTenDV.SelectedItem).Value,
                        SoLuong = Convert.ToInt32(nUDSoLuong.Value),
                        Ngay = dtpNgayDat.Value,
                        NhanVienID = this.NhanVienID
                    };
                    BLL_QLBillDV.Instance.UpdateBillDV(p);
                    ShowDGVBillDV();
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin và kiểu dữ liệu !");
                }
            }
        }
        private List<int> GetListBillDVID()
        {
            List<int> data = new List<int>();
            foreach(DataGridViewRow r in dgvBillDV.SelectedRows)
            {
                data.Add(Convert.ToInt32(r.Cells["ID"].Value.ToString()));
            }
            return data;
        }

        private void btnXoaBill_Click(object sender, EventArgs e)
        {
            if (dgvBillDV.SelectedRows.Count > 0)
            {
                BLL_QLBillDV.Instance.DeleteBillDV(GetListBillDVID());
                ShowDGVBillDV();
            }
            else
            {
                MessageBox.Show("Chọn ít nhất một dòng để xoá !");
            }
        }

        private void btnResetBill_Click(object sender, EventArgs e)
        {
            txbMaBill.Clear();
            txbMaBook.Clear();
            txbTongBill.Clear();
            nUDSoLuong.Value = 0;
            cbTenDV.SelectedIndex = 0;
        }

        private void btnSearchBill_Click(object sender, EventArgs e)
        {
            ShowDGVBillDV(txbSeachBill.Text.Trim());
        }

        private void BtnResetSBill_Click(object sender, EventArgs e)
        {
            txbSeachBill.Clear();
            ShowDGVBillDV();
        }
        private int cbTenDVIndexOf(string s)
        {
            for(int i = 0; i < cbTenDV.Items.Count; i++)
            {
                if (((CBBItem)cbTenDV.Items[i]).Value == s)
                {
                    return i;
                }
            }
            return -1;
        }
        private void dgvBillDV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            HOADON_DUNG_DICHVU p = BLL_QLBillDV.Instance.FindBillDV(Convert.ToInt32(dgvBillDV.SelectedRows[0].Cells["ID"].Value.ToString()));
            txbMaBill.Text = p.ID.ToString();
            txbMaBook.Text = p.BookID;
            cbTenDV.SelectedIndex = cbTenDVIndexOf(p.DichVuID);
            nUDSoLuong.Value = p.SoLuong;
            dtpNgayDat.Value = p.Ngay;
            txbTongBill.Text = p.ThanhTien.ToString();
            txbGiaBill.Text = p.LOAIDICHVU.DonGia.ToString();
        }
        private List<int> GetAllDGVHoaDonDichVuID()
        {
            List<int> data = new List<int>();
            foreach(DataGridViewRow r in dgvBillDV.Rows)
            {
                data.Add(Convert.ToInt32(r.Cells["ID"].Value.ToString()));
            }
            return data;
        }
        private void cbSortBillDV_DropDownClosed(object sender, EventArgs e)
        {
            if(cbSortBillDV.SelectedIndex != -1)
            {
                dgvBillDV.DataSource = BLL_QLBillDV.Instance.GetListBillDV_View(
                    BLL_QLBillDV.Instance.Sort(cbSortBillDV.SelectedItem.ToString(), GetAllDGVHoaDonDichVuID()));
            }
        }

        private void btnSortBillDV_Click(object sender, EventArgs e)
        {
            if (cbSortBillDV.SelectedIndex != -1)
            {
                dgvBillDV.DataSource = BLL_QLBillDV.Instance.GetListBillDV_View(
                    BLL_QLBillDV.Instance.Sort(cbSortBillDV.SelectedItem.ToString(), GetAllDGVHoaDonDichVuID()));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một mục để sắp xếp !");
            }
        }
        private void txbSeachBill_TextChanged(object sender, EventArgs e)
        {
            btnSearchBill.PerformClick();
        }
        #endregion

        #region Quản lý Khách hàng
        private void GuiKhachHang()
        {
            ShowDgvKh();
            cbSortKH.Items.AddRange(new string[] { "Name", "ID" });
            cbSortKH.SelectedIndex = 0;
        }
        private void ShowDgvKh()
        {
            dgvKhachHang.DataSource = BLL_QLKH.Instance.GetAlllKhView(BLL_QLKH.Instance.GetAllKhachHang());
            dgvKhachHang.Columns["KhachHangID"].HeaderText = "Mã khách hàng";
            dgvKhachHang.Columns["Ten"].HeaderText = "Tên khách hàng";
            dgvKhachHang.Columns["GioiTinh"].HeaderText = "Giới tính";
            dgvKhachHang.Columns["SDT"].HeaderText = "SĐT";
            dgvKhachHang.Columns["QuocTich"].HeaderText = "Quốc tịch";
            dgvKhachHang.Columns["GhiChu"].HeaderText = "Ghi chú";
        }
        private void btnThemKh_Click(object sender, EventArgs e)
        {
            if (txbMaKhach.Text.Length > 0)
            {
                MessageBox.Show("Vui lòng reset lại giá trị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((txbHoTen.Text.Trim()!="")&&(txbDienThoai.Text.Trim()!="")&&(txbCMND.Text.Trim()!=""))
            {    
                KHACHHANG s = new KHACHHANG()
                {

                    KhachHangID = txbMaKhach.Text,
                    Ten = txbHoTen.Text,
                    GioiTinh = rdbMale.Checked ? true : false,
                    CMND = txbCMND.Text,
                    SDT = txbDienThoai.Text,
                    QuocTich = txbQuocTich.Text,
                    GhiChu = txbGhiChu.Text
                };
            BLL_QLKH.Instance.AddKh(s);
            ShowDgvKh();
            Send();
        }
        else
        {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        }

        private void btnSuaKh_Click(object sender, EventArgs e)
        {
            if (txbMaKhach.Text.Length==0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            KHACHHANG s = new KHACHHANG()
            {

                KhachHangID = txbMaKhach.Text,
                Ten = txbHoTen.Text,
                GioiTinh = rdbMale.Checked ? true : false,
                CMND = txbCMND.Text,
                SDT = txbDienThoai.Text,
                QuocTich = txbQuocTich.Text,
                GhiChu = txbGhiChu.Text
            };
            BLL_QLKH.Instance.UpdateKh(s);
            ShowDgvKh();
            Send();
        }

        private void BtnXoaKh_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Xác nhận xóa", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    BLL_QLKH.Instance.DeleteKh(GetListKh());
                    ShowDgvKh();
                    ResetKh();
                    Send();
                }
            }
            else
            {
                MessageBox.Show("Chọn ít nhất một khách hàng để xóa!");
            }
        }

        private void btnResetKh_Click(object sender, EventArgs e)
        {
            ResetKh();
        }
        private void ResetKh()
        {
            txbMaKhach.Text = "";
            txbHoTen.Text = "";
            rdbMale.Checked = true;
            txbCMND.Text = "";
            txbDienThoai.Text = "";
            txbQuocTich.Text = "";
            txbGhiChu.Text = "";
        }
        private void btnSearchKh_Click(object sender, EventArgs e)
        {
            dgvKhachHang.DataSource = BLL_QLKH.Instance.FindKhByName(txbSeachkh.Text);
        }

        private void btnResetSkh_Click(object sender, EventArgs e)
        {
            txbSeachkh.Text = "";
            ShowDgvKh();

        }
        private List<string> GetListKh()
        {
            List<string> data = new List<string>();
            foreach (DataGridViewRow r in dgvKhachHang.SelectedRows)
            {
                data.Add(r.Cells["KhachHangID"].Value.ToString());
            }
            return data;
        }
        private void dgvKhachHang_Click(object sender, EventArgs e)
        {
            try
            {
                KHACHHANG p = BLL_QLKH.Instance.FindKh(dgvKhachHang.SelectedRows[0].Cells["KhachHangID"].Value.ToString());
                txbMaKhach.Text = p.KhachHangID;
                txbHoTen.Text = p.Ten;
                rdbMale.Checked=(bool)p.GioiTinh? true:false;
                rdbFemale.Checked = (bool)p.GioiTinh ? false : true;
                txbCMND.Text = p.CMND;
                txbDienThoai.Text = p.SDT;
                txbQuocTich.Text = p.QuocTich;
                txbGhiChu.Text = p.GhiChu;
            }
            catch { }
        }
        private void btnSortKH_Click(object sender, EventArgs e)
        {
            List<KH_View> s=BLL_QLKH.Instance.FindKhByName(txbSeachkh.Text);
            switch (cbSortKH.SelectedIndex)
            {
                case 0:                    
                    BLL_QLKH.Instance.Sort(s,"Name");
                    dgvKhachHang.DataSource = s;
                    break;
                case 1:
                    BLL_QLKH.Instance.Sort(s, "ID");
                    dgvKhachHang.DataSource = s;
                    break;
            }
        }

        private void dgvKhachHang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKhachHang.Columns[e.ColumnIndex].Name == "GioiTinh")
            {
                if (e.Value != null)
                {
                    bool gender = Convert.ToBoolean(e.Value);
                    if (gender)
                    {
                        e.Value = "Nam";
                    }
                    else
                    {
                        e.Value = "Nữ";
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        #endregion
        private void OnlyNumber(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!Char.IsDigit(e.KeyChar) && (e.KeyChar != 8));
        }
        private void OnlyCharacter(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!Char.IsLetter(e.KeyChar) && (e.KeyChar != 8) && (!Char.IsWhiteSpace(e.KeyChar)));
        }
    }
}
