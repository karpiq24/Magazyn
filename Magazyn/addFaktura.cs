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
    public partial class addFaktura : Form
    {
        public List<towar> towary;
        public List<faktura> faktury;
        private List<string> towaryS;
        double wartosc;
        int vat;
        
        public addFaktura(List<string> dostawcy, List<towar> _towary, List<faktura> _faktury, int _vat)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            towary = _towary;
            faktury = _faktury;
            towaryS = new List<string>();
            bindingSource1.DataSource = dostawcy;
            comboBox2.DataSource = bindingSource1.DataSource;
            vat = _vat;

            if(dostawcy.Exists(x=>x == "Inter Cars"))
                comboBox2.Text = "Inter Cars";
        }

        private void addFaktura_Load(object sender, EventArgs e)
        {
            wartosc = 0;
            textBox3.Text = wartosc.ToString() + " zł";
            String tekst;
            for (int i = 0; i < towary.Count; i++)
            {
                tekst = "";
                tekst += towary[i].indeks;
                tekst += " | " + towary[i].nazwa;

                towaryS.Add(tekst);
            }
            bindingSource2.DataSource = towaryS;
            comboBox1.DataSource = bindingSource2;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            faktury = faktury.OrderByDescending(x => x.data).ToList();  
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            double netto;
            if (double.TryParse(textBox2.Text, out netto))
            {
            }
            else if (textBox2.Text.Length > 0)
            {
                textBox2.Text = textBox2.Text.Remove(textBox2.SelectionStart - 1, 1);
                textBox2.Select(textBox2.Text.Length, 0);
            }
        }

        private string dodajTow(string ind)
        {
            int ile = towary.Count;
            addTowary dodajT;
            string txt = "";
            if(ind == "")
                dodajT = new addTowary(towary, vat);
            else
                dodajT = new addTowary(towary, vat,ind);
            dodajT.ShowDialog();

            if (towary.Count > ile)
            {
                txt = dodajT.pokazIndeks();
                comboBox1.DataSource = null;
                bindingSource2.DataSource = null;
                comboBox1.Items.Clear();

                towary = dodajT.towary;
                towary = towary.OrderBy(x => x.indeks).ToList();

                towaryS = new List<string>();

                String tekst;
                for (int i = 0; i < towary.Count; i++)
                {
                    tekst = "";
                    tekst += towary[i].indeks;
                    tekst += " | " + towary[i].nazwa;

                    towaryS.Add(tekst);
                }
                bindingSource2.DataSource = towaryS;
                comboBox1.DataSource = bindingSource2;
            }
            return txt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                System.Media.SystemSounds.Hand.Play();
                DialogResult res = MessageBox.Show("Nie ma takiego indeksu w bazie. Czy dodać jako nowy towar?","Zapytanie",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    string tekst = comboBox1.Text;
                    tekst = dodajTow(comboBox1.Text);
                    comboBox1.SelectedItem = towaryS.Find(x => x.StartsWith(tekst));
                }
                else
                {
                    comboBox1.Text = "";
                }                
                return;
            }
                

            if (textBox2.Text.Length == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Podaj cenę zakupu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Select();
            }
            else
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if(item.Text == towary[comboBox1.SelectedIndex].indeks)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        MessageBox.Show("Towar o tym indeksie został już dodany.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        comboBox1.Select();
                        return;
                    }
                }

                wartosc += (double.Parse(textBox2.Text) * double.Parse(numericUpDown1.Value.ToString()));
                textBox3.Text = wartosc.ToString() + " zł";                

                ListViewItem item1 = new ListViewItem(towary[comboBox1.SelectedIndex].indeks);
                item1.SubItems.Add(towary[comboBox1.SelectedIndex].nazwa);
                item1.SubItems.Add(textBox2.Text);
                item1.SubItems.Add(numericUpDown1.Value.ToString());

                listView1.Items.Add(item1);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                textBox2.Clear();
                numericUpDown1.Value = 1;
                comboBox1.Select();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           if (listView1.Items.Count > 0 && listView1.SelectedItems.Count > 0)
           {
               wartosc -= (double.Parse(listView1.SelectedItems[0].SubItems[2].Text) * (double.Parse(listView1.SelectedItems[0].SubItems[3].Text)));
               listView1.SelectedItems[0].Remove();
               textBox3.Text = wartosc.ToString() + " zł";
           }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Podaj numer faktury!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Select();
                return;
            }
            
            if (faktury.Exists(x => x.dostawca == comboBox2.Text && x.nr_faktury == textBox1.Text))
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Faktura tego dostawcy o tym numerze istnieje już w bazie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox1.Select();
            }
            else
            {
                faktura nowa = new faktura();
                nowa.data = dateTimePicker1.Value.Date;
                nowa.dostawca = comboBox2.Text;
                nowa.nr_faktury = textBox1.Text;
                nowa.wartosc = wartosc;

                nowa.przedmioty = new List<rzecz>();

                foreach (ListViewItem item in listView1.Items)
                {
                    rzecz p;
                    p.cena = double.Parse(item.SubItems[2].Text);
                    p.ile = int.Parse(item.SubItems[3].Text);
                    p.indeks = item.Text;
                    nowa.przedmioty.Add(p);

                    towary[towary.FindIndex(x => x.indeks == p.indeks)].ostatni = p.cena;
                }

                faktury.Add(nowa);

                listView1.Items.Clear();
                textBox1.Clear();
                wartosc = 0;
                textBox3.Text = wartosc.ToString() + " zł";
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Dodano fakturę do bazy.", "Powiadomienie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Select();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (faktury.Exists(x => x.dostawca == comboBox2.Text && x.nr_faktury == textBox1.Text))
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Faktura tego dostawcy o tym numerze istnieje już w bazie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox1.Select();
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            bool br = false;
            string ind = comboBox1.Text.Substring(0,comboBox1.Text.IndexOf(" |"));
            for (int i = 0; i < faktury.Count; i++)
            {
                for (int j = 0; j < faktury[i].przedmioty.Count; j++)
                {
                    if(faktury[i].przedmioty[j].indeks == ind)
                    {
                        textBox2.Text = faktury[i].przedmioty[j].cena.ToString();
                        br = true;
                        break;
                    }
                }
                if (br)
                    break;
            }

        }


    }
}
