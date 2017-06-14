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
    public partial class edytujTowar : Form
    {
        public List<towar> towary;
        string ind;
        int nr;
        int vat;

        public edytujTowar(List<towar> _towary, int _nr, int _vat)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            towary = _towary;
            nr = _nr;
            vat = _vat;
        }

        private void edytujTowar_Load(object sender, EventArgs e)
        {
            ind = towary[nr].indeks;
            textBox1.Text = towary[nr].indeks;
            comboBox1.Text = towary[nr].nazwa;
            textBox3.Text = towary[nr].netto.ToString();
            textBox4.Text = towary[nr].brutto.ToString();
            textBox5.Text = towary[nr].uwagi;
            numericUpDown1.Value = towary[nr].stan;

            List<string> nazwy = new List<string>();
            foreach (towar item in towary)
            {
                if (!nazwy.Exists(x => x == item.nazwa))
                {
                    nazwy.Add(item.nazwa);
                }
            }

            comboBox1.Items.AddRange(nazwy.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(ind != textBox1.Text)
            {
                if (towary.Exists(x => x.indeks == textBox1.Text))
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    MessageBox.Show("Towar o podanym indeksie istnieje już w bazie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                    textBox1.Select();
                }
                else
                {
                    towary[nr].indeks = textBox1.Text;
                    towary[nr].nazwa = comboBox1.Text;

                    if (textBox3.Text.Length == 0)
                    {
                        towary[nr].netto = 0;
                        towary[nr].brutto = 0;
                    }
                    else
                    {
                        towary[nr].netto = float.Parse(textBox3.Text);
                        towary[nr].brutto = float.Parse(textBox4.Text);
                    }

                    towary[nr].uwagi = textBox5.Text;
                    towary[nr].stan = (uint)numericUpDown1.Value;
                    this.Close();
                }
            }
            else
            {
                towary[nr].nazwa = comboBox1.Text;

                if (textBox3.Text.Length == 0)
                {
                    towary[nr].netto = 0;
                    towary[nr].brutto = 0;
                }
                else
                {
                    towary[nr].netto = float.Parse(textBox3.Text);
                    towary[nr].netto = Math.Round(towary[nr].netto, 2);
                    towary[nr].brutto = float.Parse(textBox4.Text);
                    towary[nr].brutto = Math.Round(towary[nr].brutto, 2);
                }

                towary[nr].uwagi = textBox5.Text;
                towary[nr].stan =(uint) numericUpDown1.Value;
                this.Close();
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Hand.Play();
            DialogResult res = MessageBox.Show("Na pewno skasować towar?", "Potwierdź", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                towary.RemoveAt(nr);
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                double netto;
                if (double.TryParse(textBox3.Text, out netto))
                {
                    double brutto;
                    double v = (double)vat / 100;
                    brutto = Math.Round(netto * (1 + v), 2);
                    textBox4.Text = brutto.ToString();
                }
                else if (textBox3.Text.Length > 0)
                {
                    textBox3.Text = textBox3.Text.Remove(textBox3.SelectionStart - 1, 1);
                    textBox3.Select(textBox3.Text.Length, 0);
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                double brutto;
                if (double.TryParse(textBox4.Text, out brutto))
                {
                    double netto;
                    netto = Math.Round((brutto / (100 + vat)) * 100, 2);
                    textBox3.Text = netto.ToString();
                }
                else if (textBox4.Text.Length > 0)
                {
                    textBox4.Text = textBox4.Text.Remove(textBox4.SelectionStart - 1, 1);
                    textBox4.Select(textBox4.Text.Length, 0);
                }
            }
        }
    }
}
