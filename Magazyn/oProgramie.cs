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
    public partial class oProgramie : Form
    {
        public oProgramie()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_DoubleClick(object sender, EventArgs e)
        {
            timer1.Start();
            label1.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            pictureBox2.Hide();
            
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rndloc();
            pictureBox1.Show();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            timer3.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Hand.Play();
        }

        private void oProgramie_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer2.Start();
            timer3.Stop();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void rndloc()
        {
            if (timer1.Enabled)
            {
                Random rnd = new Random();
                int x, y;
                x = rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - this.Size.Width);
                y = rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - this.Size.Height);
                this.Location = new Point(x, y);
            }
        }
        private void oProgramie_MouseHover(object sender, EventArgs e)
        {
            rndloc();
        }

        private void oProgramie_MouseEnter(object sender, EventArgs e)
        {
            rndloc();
        }

        private void oProgramie_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.klima-kar.pl/");
        }
    }
}
