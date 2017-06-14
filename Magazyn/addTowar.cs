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
    public partial class addTowary : Form
    {
        towar przedmiot;
        public List<towar> towary;
        int vat;
        
        public addTowary(List<towar> _towary, int _vat)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            towary = _towary;
            vat = _vat;
        }

        public addTowary(List<towar> _towary, int _vat, string _ind)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            towary = _towary;
            vat = _vat;
            textBox1.Text = _ind;
        }

        private void addTowar_Load(object sender, EventArgs e)
        {
            List<string> nazwy = new List<string>();
            foreach (towar item in towary)
            {
                if(!nazwy.Exists(x => x == item.nazwa))
                {
                    nazwy.Add(item.nazwa);
                }
            }

            comboBox1.Items.AddRange(nazwy.ToArray());
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public string pokazIndeks()
        {
            return przedmiot.indeks;
        }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                przedmiot = new towar();
                przedmiot.indeks = textBox1.Text;
                przedmiot.nazwa = comboBox1.Text;
                przedmiot.uwagi = textBox5.Text;

                if (textBox3.Text.Length == 0)
                {
                    przedmiot.netto = 0;
                    przedmiot.brutto = 0;
                }
                else
                {
                    przedmiot.netto = float.Parse(textBox3.Text);
                    przedmiot.netto = Math.Round(przedmiot.netto, 2);
                    przedmiot.brutto = float.Parse(textBox4.Text);
                    przedmiot.brutto = Math.Round(przedmiot.brutto, 2);
                }

                przedmiot.stan = (uint)numericUpDown1.Value;

                towary.Add(przedmiot);

                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Dodano towar do bazy.", "Powiadomienie", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Text = "";
                comboBox1.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                numericUpDown1.Value = 1;
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Podaj indeks towaru.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Select();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(towary.Exists(x => x.indeks == textBox1.Text))
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Towar o podanym indeksie istnieje już w bazie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
                labelStan.Visible = true;
            else
                labelStan.Visible = false;
        }
    }
}
