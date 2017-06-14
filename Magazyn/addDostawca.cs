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
    public partial class addDostawca : Form
    {
        public List<string> dostawcy;
        public addDostawca(List<string> _dostawcy)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            dostawcy = _dostawcy;
        }

        private void addDostawca_Load(object sender, EventArgs e)
        {
            bindingSource1.DataSource = dostawcy;
            listBox1.DataSource = bindingSource1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dostawcy.Exists(x => x == textBox1.Text))
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Ten dostawca już jest w bazie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox1.Select();
            }
            else if(textBox1.Text.Length > 0)
            {
                bindingSource1.Add(textBox1.Text);
                textBox1.Text = "";
                textBox1.Select();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bindingSource1.RemoveAt(listBox1.SelectedIndex);
        }
    }
}
