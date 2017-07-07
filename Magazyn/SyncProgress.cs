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
    public partial class SyncProgress : Form
    {
        public SyncProgress()
        {
            InitializeComponent();
        }

        private void SyncProgress_Load(object sender, EventArgs e)
        {

        }

        public void setDates(string text)
        {
            textBox1.Text = text;
            textBox1.Refresh();
        }

        public void setUpProgressBar(int max)
        {
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
        }

        public void setProgress(int value)
        {
            progressBar1.Value = value;
            progressBar1.Refresh();
        }

        public void appendLog(string text)
        {
            richTextBox1.AppendText(text + "\n");
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
            richTextBox1.Refresh();
        }
    }
}
