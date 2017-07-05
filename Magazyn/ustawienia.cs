using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magazyn
{
    public partial class ustawienia : Form
    {
        public int vat;
        public bool zapis, wczyt;
        public string firma;
        public string IC_token;
        public string IC_number;
        public ustawienia()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            vat =(int) numericUpDown1.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            zapis = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            wczyt = checkBox2.Checked;
        }

        private void ustawienia_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = vat;
            checkBox1.Checked = zapis;
            checkBox2.Checked = wczyt;
            textBox1.Text = firma;
            textBox2.Text = IC_number;
            textBox3.Text = IC_token;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            IC_number = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            IC_token = textBox3.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            firma = textBox1.Text;
        }
    }
}
