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
    public partial class towarFaktury : Form
    {
        string indeks;
        List<faktura> faktury;

        public towarFaktury(List<faktura> _faktury, string _indeks)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            indeks = _indeks;
            faktury = _faktury;
        }

        private void towarFaktury_Load(object sender, EventArgs e)
        {
            textBox1.Text = indeks;

            foreach (faktura fak in faktury)
            {
                foreach (rzecz item in fak.przedmioty)
                {
                    if (item.indeks == indeks)
                    {
                        ListViewItem item1 = new ListViewItem(fak.dostawca);
                        item1.SubItems.Add(fak.nr_faktury);
                        item1.SubItems.Add(fak.data.ToShortDateString());
                        item1.SubItems.Add(item.ile.ToString());
                        item1.SubItems.Add(item.cena.ToString());
                        double razem = item.ile * item.cena;
                        item1.SubItems.Add(razem.ToString());
                        listView1.Items.Add(item1);
                    }
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
