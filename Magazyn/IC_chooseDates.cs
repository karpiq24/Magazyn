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
    public partial class IC_chooseDates : Form
    {
        public bool sync;
        public bool full;
        public int start, end;
        public IC_chooseDates()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
            }
            else
            {
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sync = true;
            full = checkBox1.Checked;
            start = (int)numericUpDown1.Value;
            end = (int)numericUpDown1.Value;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
