using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username.Equals("admin") && password.Equals("admin"))
            {

                Form3 form1 = new Form3();
                this.Hide();

                form1.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username or Password is incorrect!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult f = MessageBox.Show("Bạn muốn thoát không ?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            {
                this.Close();
                Application.Exit();
            }
        }
    }
}
