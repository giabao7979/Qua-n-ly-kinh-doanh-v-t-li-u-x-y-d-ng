using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class Form3 : Form
    {
        int index = 0;
        SqlConnection connection;
        string connectionString = @"Data Source=EUGEN\GIABAO;Initial Catalog=master;User ID=sa;Password=123 ;MultipleActiveResultSets=true;";
        public Form3()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            connection.Open();

            loadData();
            loadDataHoaDon();
            loadDataThongKeHoaDon();
        }


           private List<SanPham> readData()
        {
            List<SanPham> data = new List<SanPham>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = "Select * from [tbl_products]";
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new SanPham(reader.GetString(0), reader.GetString(1), reader.GetInt64(2),
                    reader.GetInt32(3), reader.GetString(4), reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
            }

            // to close all the objects
            reader.Close();
            cmd.Dispose();

            return data;
        }

        private List<SanPham> getByID(String maSP)
        {
            List<SanPham> data = new List<SanPham>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = String.Format("Select * from [tbl_products] where MaSP = '{0}'", maSP);
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new SanPham(reader.GetString(0), reader.GetString(1), reader.GetInt64(2),
                    reader.GetInt32(3), reader.GetString(4), reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
            }

            // to close all the objects
            reader.Close();
            cmd.Dispose();

            return data;
        }

        private List<SanPham> getByHoaDonID(String hoaDonID)
        {
            List<SanPham> data = new List<SanPham>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = String.Format("Select p.* from tbl_products p inner join tbl_orders h on p.MaSP = h.MaSP where h.MaHD = '{0}'", hoaDonID);
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new SanPham(reader.GetString(0), reader.GetString(1), reader.GetInt64(2),
                    reader.GetInt32(3), reader.GetString(4), reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
            }

            // to close all the objects
            reader.Close();
            cmd.Dispose();

            return data;
        }




        private void saveData(SanPham kh)
        {
            SqlCommand cmd;
            string insertCommand = String.Format(
                "insert into tbl_products values ('{0}', N'{1}', {2}, {3}, N'{4}', '{5}', N'{6}', N'{7}')",
                kh.MaSP, kh.TenSP, kh.GiaSP, kh.SoLuong, kh.DonVi, "2022-11-20", kh.NhaCungCap, kh.TrangThai);
            string updateCommand = String.Format(
                "update tbl_products set TenSP = N'{0}', GiaSP = {1}, SoLuong = {2}, DonVi = N'{3}', NhaCC = N'{4}', TrangThai = N'{5}' " +

                " where MaSP = '{6}'",
                kh.TenSP, kh.GiaSP, kh.SoLuong, kh.DonVi, kh.NhaCungCap, kh.TrangThai, kh.MaSP);
            if (getByID(kh.MaSP).Count > 0)
                cmd = new SqlCommand(updateCommand, connection);
            else
                cmd = new SqlCommand(insertCommand, connection);
            
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void deleteByMaSP(string MaSP)
        {
            SqlCommand cmd;
            string sqlOrder = "Delete from [tbl_orders] where MaSP = '" + MaSP + "'";
            cmd = new SqlCommand(sqlOrder, connection);
            cmd.ExecuteNonQuery();
            string sql = "Delete from [tbl_products] where MaSP = '" + MaSP + "'";
            cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        
        private void loadData()
        {
            listView1.Items.Clear();
            List<SanPham> datas = readData();
            foreach(SanPham e in datas)
            {
                ListViewItem item = new ListViewItem(e.MaSP.ToString());
 
                item.SubItems.Add(e.TenSP.ToString());
                item.SubItems.Add(e.GiaSP.ToString());
                item.SubItems.Add(e.SoLuong.ToString());
                item.SubItems.Add(e.DonVi.ToString());
                item.SubItems.Add(e.NgayNhap.ToString());
                item.SubItems.Add(e.NhaCungCap.ToString());
                item.SubItems.Add(e.TrangThai.ToString());
                listView1.Items.Add(item);
            }
            
        }

        private void loadData(List<SanPham> datas)
        {
            listView1.Items.Clear();
            foreach (SanPham e in datas)
            {
                ListViewItem item = new ListViewItem(e.MaSP.ToString());

                item.SubItems.Add(e.TenSP.ToString());
                item.SubItems.Add(e.GiaSP.ToString());
                item.SubItems.Add(e.SoLuong.ToString());
                item.SubItems.Add(e.DonVi.ToString());
                item.SubItems.Add(e.NgayNhap.ToString());
                item.SubItems.Add(e.NhaCungCap.ToString());
                item.SubItems.Add(e.TrangThai.ToString());
                listView1.Items.Add(item);
            }

        }

        // DIEM----------------------------------------
        private void deleteHoaDon(string HoaDon)
        {
            SqlCommand cmd;
            string sql = String.Format("Delete from [tbl_hoadon] where MaHD = '{0}'", HoaDon);
            cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void saveDataHoaDon(HoaDon kh)
        {
            index += 1;
            SqlCommand cmd;
            string insertCommand = String.Format(
                "insert into tbl_hoadon values ('{0}', N'{1}', '{2}', N'{3}', '{4}')",
                "HD" + index, kh.TenKhachHang, kh.Sdt, kh.DiaChi, kh.TrangThai);
           
            cmd = new SqlCommand(insertCommand, connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void updateTrangThaiHoaDon(HoaDon kh)
        {
            SqlCommand cmd;
            string insertCommand = String.Format(
                "update tbl_hoadon set [TrangThai] = '{0}' where [MaHD] = '{1}'",
                kh.TrangThai, kh.MaHD);

            cmd = new SqlCommand(insertCommand, connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private List<HoaDon> getHoaDonByMaHD(String maHD)
        {
            List<HoaDon> data = new List<HoaDon>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = String.Format("Select * from [tbl_hoadon] where MaHD = '{0}'", maHD);
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HoaDon h = new HoaDon(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                    reader.GetString(3), reader.GetString(4));

                h.SanPham = getByHoaDonID(h.MaHD);
                data.Add(h);
            }

            // to close all the objects
            reader.Close();
            cmd.Dispose();

            return data;
        }

       /* private List<Diem> getDiemByMasvAndMonHoc(String masv, String monhoc)
        {
            List<Diem> data = new List<Diem>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = String.Format("Select * from [tbl_diem] where masv = '{0}' and monhoc= N'{1}'", masv, monhoc);
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Diem(getByID(reader.GetString(0))[0], reader.GetString(1), reader.GetDouble(2)));
            }

            // to close all the objects
            reader.Close();
            cmd.Dispose();

            return data;
        }*/

        private List<HoaDon> readDataHoaDon()
        {
            List<HoaDon> data = new List<HoaDon>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = "Select * from [tbl_hoadon]";
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HoaDon h = new HoaDon(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                   reader.GetString(3), reader.GetString(4));
                Console.WriteLine(reader.GetSqlString(0));
                h.SanPham = getByHoaDonID(h.MaHD);
                Console.WriteLine(h);
                data.Add(h);
            }

            // to close all the objects
            reader.Close();
            cmd.Dispose();

            return data;
        }

        private List<HoaDon> readDataHoaDonBySDT(string sdt)
        {
            List<HoaDon> data = new List<HoaDon>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = "Select * from [tbl_hoadon] where Sdt = " + sdt;
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HoaDon h = new HoaDon(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                   reader.GetString(3), reader.GetString(4));
                h.SanPham = getByHoaDonID(h.MaHD);
                data.Add(h);
            }

            reader.Close();
            cmd.Dispose();

            return data;
        }

        private List<HoaDon> readDataHoaDonByTrangThai(string trangThai)
        {
            List<HoaDon> data = new List<HoaDon>();

            SqlCommand cmd;
            SqlDataReader reader;
            string sql, output = "";
            sql = "Select * from [tbl_hoadon] where TrangThai = '" + trangThai + "'";
            cmd = new SqlCommand(sql, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HoaDon h = new HoaDon(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                   reader.GetString(3), reader.GetString(4));
                h.SanPham = getByHoaDonID(h.MaHD);
                data.Add(h);
            }

            reader.Close();
            cmd.Dispose();

            return data;
        }

        private void loadDataHoaDon()
        {
            listView2.Items.Clear();
            List<HoaDon> datas = readDataHoaDon();
            foreach (HoaDon e in datas)
            {
                Console.WriteLine("data:" + e.MaHD);
                index = Math.Max(index, Int32.Parse(e.MaHD.Substring(2)));
                ListViewItem item = new ListViewItem(e.MaHD.ToString());
                item.SubItems.Add(e.TenKhachHang.ToString());
                item.SubItems.Add(e.DiaChi.ToString());
                item.SubItems.Add(e.Sdt.ToString());
                item.SubItems.Add(e.getTotal().ToString());
                item.SubItems.Add(e.TrangThai.ToString());
                listView2.Items.Add(item);
            }

            loadDataToComboBox();

        }

        private void loadDataToComboBox()
        {
            comboBox1.Items.Clear();
            List<SanPham> datas = readData();
            foreach (SanPham e in datas)
            {
                comboBox1.Items.Add(e.MaSP + "-" + e.TenSP);

            }
            if (datas.Count > 0) 
            comboBox1.SelectedIndex = 0;

        }
        private void loadDataHoaDon(List<HoaDon> datas)
        {
            listView2.Items.Clear();
            foreach (HoaDon e in datas)
            {

                ListViewItem item = new ListViewItem(e.MaHD.ToString());
                item.SubItems.Add(e.TenKhachHang.ToString());
                item.SubItems.Add(e.DiaChi.ToString());
                item.SubItems.Add(e.Sdt.ToString());
                item.SubItems.Add(e.getTotal().ToString());
                item.SubItems.Add(e.TrangThai.ToString());
                listView2.Items.Add(item);
            }

        }

        // -------------------- 
        public void addOrder(string maHD, string maSP)
        {
            SqlCommand cmd;
            Console.WriteLine(maHD);
            string insertCommand = String.Format(
                "insert into tbl_orders values ('{0}', '{1}')",
                maSP, maHD);

            cmd = new SqlCommand(insertCommand, connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void loadDataThongKeHoaDon()
        {
            listView3.Items.Clear();
            List<HoaDon> datas = readDataHoaDon();
            foreach (HoaDon e in datas)
            {
                Console.WriteLine("data:" + e.MaHD);
                index = Math.Max(index, Int32.Parse(e.MaHD.Substring(2)));
                ListViewItem item = new ListViewItem(e.MaHD.ToString());
                item.SubItems.Add(e.TenKhachHang.ToString());
                item.SubItems.Add(e.DiaChi.ToString());
                item.SubItems.Add(e.Sdt.ToString());
                item.SubItems.Add(e.getTotal().ToString());
                item.SubItems.Add(e.TrangThai.ToString());
                listView3.Items.Add(item);
            }

            loadDataToComboBox();

        }

     
        private void loadDataThongKeHoaDon(List<HoaDon> datas)
        {
            listView3.Items.Clear();
            foreach (HoaDon e in datas)
            {

                ListViewItem item = new ListViewItem(e.MaHD.ToString());
                item.SubItems.Add(e.TenKhachHang.ToString());
                item.SubItems.Add(e.DiaChi.ToString());
                item.SubItems.Add(e.Sdt.ToString());
                item.SubItems.Add(e.getTotal().ToString());
                item.SubItems.Add(e.TrangThai.ToString());
                listView3.Items.Add(item);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           // public SanPham(string maSP, string tenSP, long giaSP, int soLuong, string donVi, string ngayNhap, string nhaCungCap, string trangThai)

            SanPham kh = new SanPham(
               textBoxMaSP.Text,
               textBoxTenSP.Text,
              
               Int64.Parse(textBoxGiaSp.Text),
               Int32.Parse(textboxSoLuong.Text),
               comboBoxDonVi.GetItemText(comboBoxDonVi.SelectedItem),
                dateNgayNhap.Value,
               textBoxNhaCungCap.Text,
               comboBoxTrangThai.GetItemText(comboBoxTrangThai.SelectedItem)
            );
            Console.WriteLine(kh);
            saveData(kh);
            loadData();
            clearData();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                textBoxMaSP.Text = listView1.SelectedItems[0].SubItems[0].Text;
                textBoxTenSP.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBoxGiaSp.Text = listView1.SelectedItems[0].SubItems[2].Text;
                textboxSoLuong.Text = listView1.SelectedItems[0].SubItems[3].Text;
                comboBoxDonVi.SelectedIndex = comboBoxDonVi.FindString(listView1.SelectedItems[0].SubItems[4].Text);
                //.Text = listView1.SelectedItems[0].SubItems[5].Text;
                textBoxNhaCungCap.Text = listView1.SelectedItems[0].SubItems[6].Text;
                comboBoxTrangThai.SelectedIndex = comboBoxTrangThai.FindString(listView1.SelectedItems[0].SubItems[7].Text);
                

            }
        }

        private void textBoxSoTien_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxLoai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadData(getByID(textBoxFindMaSP.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // loadData(getByLop(textBoxFindLop.Text));
            List<SanPham> sp = readData();
            sp.Sort((s1, s2) => s1.GiaSP.CompareTo(s2.GiaSP));
            loadData(sp);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count > 0)
            {
                string MaKH = listView2.SelectedItems[0].SubItems[0].Text;
                deleteHoaDon(MaKH);
                loadDataHoaDon();
                loadDataThongKeHoaDon();
                clearDataHoaDon();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {


            HoaDon sv = new HoaDon(
                null,
                textBoxDiemTenKH.Text,
                textBoxSdt.Text,
                textBoxDiaChi.Text,
                "Chua TT"
                );

            saveDataHoaDon(sv);
            loadDataHoaDon();
            clearDataHoaDon();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            loadDataHoaDon(readDataHoaDonBySDT(textBoxDiemFindSDT.Text));
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void clearData()
        {
            textBoxMaSP.Text = "";
            textBoxTenSP.Text = "";
            textboxSoLuong.Text = "";
            textBoxGiaSp.Text = "";
            textBoxNhaCungCap.Text = "";
        }

        private void clearDataHoaDon()
        {
            textBoxDiemTenKH.Text = "";
            textBoxSdt.Text = "";
            textBoxDiaChi.Text = "";
            textBoxOrderMaHD.Text = "";
            hoaDonSelected = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                string MaKH = listView1.SelectedItems[0].SubItems[0].Text;
                deleteByMaSP(MaKH);
                loadData();
                loadDataHoaDon();
                loadDataThongKeHoaDon();
                clearData();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            clearData();
            loadData();
        }

        string hoaDonSelected = null;

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count > 0)
            {
                hoaDonSelected = listView2.SelectedItems[0].SubItems[0].Text;
                textBoxDiemTenKH.Text = listView2.SelectedItems[0].SubItems[1].Text;
                textBoxSdt.Text = listView2.SelectedItems[0].SubItems[3].Text;
                textBoxDiaChi.Text = listView2.SelectedItems[0].SubItems[2].Text;
                textBoxOrderMaHD.Text = listView2.SelectedItems[0].SubItems[0].Text;

                HoaDon hd = getHoaDonByMaHD(hoaDonSelected)[0];

                string sps = "";
                foreach(SanPham sp in hd.SanPham)
                {
                    sps += sp.ToString();
                    sps += "\n";
                }

                MessageBox.Show(String.Format("Mã HĐ: {0}\nTên KH: {1}\nSĐT: {2}\n----DANH SÁCH SP----\n{3}\n----------\n" +
                    "=> Total: {4}\n => Trạng thái: {5}"
                    , hd.MaHD, hd.TenKhachHang, hd.Sdt, sps, hd.getTotal(), hd.TrangThai, "Chi tiết hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Information)); ;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            clearDataHoaDon();
            loadDataHoaDon();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxNganh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxDonVi_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            List<SanPham> sp = readData();
            sp.Sort((s1, s2) => s2.SoLuong.CompareTo(s1.SoLuong));
            loadData(sp);
        }

        private void textBoxMonHOc_TextChanged(object sender, EventArgs e)
        {

        }


        private void button11_Click(object sender, EventArgs e)
        {
            HoaDon hd = getHoaDonByMaHD(hoaDonSelected)[0];

            hd.TrangThai = "Da TT";
            updateTrangThaiHoaDon(hd);
            loadDataHoaDon();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if(hoaDonSelected == null)
            {
                MessageBox.Show("Please chọn hóa đơn trước!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            return;
            }
            string selected = comboBox1.GetItemText(comboBox1.SelectedItem);
            Console.Write("Selected: " + selected);
            string maSP = selected.Substring(0, selected.IndexOf("-"));

            addOrder(hoaDonSelected, maSP);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            loadDataThongKeHoaDon(readDataHoaDonBySDT(textBox1.Text));

        }

        private void button15_Click(object sender, EventArgs e)
        {
            loadDataThongKeHoaDon(readDataHoaDonByTrangThai("Chua TT"));

        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            loadDataThongKeHoaDon(readDataHoaDonByTrangThai("Da TT"));

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }

    class SanPham
    {
        public String MaSP { get; set; }
        public String TenSP { get; set; }
        public long GiaSP { get; set; }
        public int SoLuong { get; set; }
        public String DonVi { get; set; }
        public DateTime NgayNhap { get; set; }
        public String NhaCungCap { get; set; }
        public String TrangThai { get; set; }

        public SanPham()
        {
        }

        public SanPham(string maSP, string tenSP, long giaSP, int soLuong, string donVi, DateTime ngayNhap, string nhaCungCap, string trangThai)
        {
            MaSP = maSP;
            TenSP = tenSP;
            GiaSP = giaSP;
            SoLuong = soLuong;
            DonVi = donVi;
            NgayNhap = ngayNhap;
            NhaCungCap = nhaCungCap;
            TrangThai = trangThai;
        }

        public String ToString()
        {
            return TenSP + " - " + GiaSP;
        }
    }

    class HoaDon
    {
        public String MaHD { get; set; }

        public String TenKhachHang { get; set; }

        public String Sdt { get; set; }

        public String DiaChi { get; set; }

        public String TrangThai { get; set; }
        public List<SanPham> SanPham { get; set; }

        public HoaDon(string maHD, string tenKhachHang, string sdt, string diaChi, string trangThai)
        {
            MaHD = maHD;
            TenKhachHang = tenKhachHang;
            Sdt = sdt;
            DiaChi = diaChi;
            TrangThai = trangThai;
        }

        public long getTotal()
        {
            long total = 0;
            foreach(SanPham sp in SanPham)
            {
                total += sp.GiaSP;
            }
            return total;
        }


    }
}
