using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Globalization;

namespace Magazyn
{
    public partial class Form1 : Form
    {
        List<faktura> faktury;
        List<towar> towary;
        List<string> dostawcy;
        List<string> towaryS;
        ustawienia settings;
        int wlaczony;

        int sortT,sortF;
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }

        //Start programu
        private void Form1_Load(object sender, EventArgs e)
        {
            dostawcy = new List<string>();
            faktury = new List<faktura>();
            towary = new List<towar>();
            towaryS = new List<string>();
            settings = new ustawienia();
            sortF = sortT = 1;

            bool sciezka;
            string path = "baza";
            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
                System.IO.Directory.CreateDirectory(path);
            wlaczony = 1;
            StreamReader r;
            sciezka = File.Exists(@"baza\ustawienia.srk");
            if (sciezka)
            {
                r = new StreamReader(@"baza\ustawienia.srk");
                settings.vat = int.Parse(r.ReadLine());
                settings.zapis = bool.Parse(r.ReadLine());
                settings.wczyt = bool.Parse(r.ReadLine());
                settings.firma = r.ReadLine();
                wlaczony = int.Parse(r.ReadLine());
                settings.IC_token = r.ReadLine();
                settings.IC_number = r.ReadLine();
                wlaczony++;
                r.Close();


                if (wlaczony > 1)
                {
                    MessageBox.Show("Program jest już uruchomiony.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                else
                {
                    zapiszUst();         
                }

                label1.Text = settings.firma;
                label2.Text = settings.firma;

                if (settings.firma.Length > 0)
                    this.Text = "Magazyn - " + settings.firma;
                else
                    this.Text = "Magazyn";

                if (settings.wczyt)
                    wczytaj();
            }
            else
            {
                zapiszUst();         
            }

            ListViewItem item1;

            for (int i = 0; i < towary.Count; i++)
            {
                item1 = new ListViewItem(towary[i].indeks);
                item1.SubItems.Add(towary[i].nazwa);
                item1.SubItems.Add(towary[i].netto.ToString() + " zł");
                item1.SubItems.Add(towary[i].brutto.ToString() + " zł");
                item1.SubItems.Add(towary[i].stan.ToString());
                item1.SubItems.Add(towary[i].uwagi);
                item1.SubItems.Add(towary[i].ostatni.ToString() + " zł");

                listView1.Items.Add(item1);
            }


            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            for (int i = 0; i < faktury.Count; i++)
            {
                item1 = new ListViewItem(faktury[i].dostawca);
                item1.SubItems.Add(faktury[i].nr_faktury);
                item1.SubItems.Add(faktury[i].data.ToShortDateString());
                item1.SubItems.Add(faktury[i].wartosc.ToString() + " zł");              

                listView2.Items.Add(item1);
            }
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);


            listView1.ContextMenuStrip = contextMenuStrip1;
            listView2.ContextMenuStrip = contextMenuStrip2;

            textBox1.Select();
        }

        //Odświeżanie listy towarów
        private void odswiezT()
        {
            listView1.Items.Clear();
            List<towar> sztowary;
            sztowary = towary.FindAll(x =>
                x.indeks.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.nazwa.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 x.uwagi.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            if(checkBox1.Checked)
            {
                sztowary = sztowary.FindAll(x => x.stan > 0);
            }

            if (sztowary.Count == 0)
                System.Media.SystemSounds.Exclamation.Play();

            ListViewItem item1;
            for (int i = 0; i < sztowary.Count; i++)
            {
                item1 = new ListViewItem(sztowary[i].indeks);
                item1.SubItems.Add(sztowary[i].nazwa);
                item1.SubItems.Add(sztowary[i].netto.ToString() + " zł");
                item1.SubItems.Add(sztowary[i].brutto.ToString() + " zł");
                item1.SubItems.Add(sztowary[i].stan.ToString());
                item1.SubItems.Add(sztowary[i].uwagi);
                item1.SubItems.Add(sztowary[i].ostatni.ToString() + " zł");
                listView1.Items.Add(item1);
            }


            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        //Odświeżanie listy faktur
        private void odswiezF()
        {
            listView2.Items.Clear();
            List<faktura> szfaktury;
            szfaktury = faktury.FindAll(x =>
                x.dostawca.IndexOf(textBox2.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.data.ToShortDateString().IndexOf(textBox2.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 x.nr_faktury.IndexOf(textBox2.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            if (szfaktury.Count == 0)
                System.Media.SystemSounds.Exclamation.Play();

            ListViewItem item1;
            for (int i = 0; i < szfaktury.Count; i++)
            {
                item1 = new ListViewItem(szfaktury[i].dostawca);
                item1.SubItems.Add(szfaktury[i].nr_faktury);
                item1.SubItems.Add(szfaktury[i].data.ToShortDateString());
                item1.SubItems.Add(szfaktury[i].wartosc.ToString() + " zł");

                listView2.Items.Add(item1);
            }

            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        //Dodawanie towaru
        private void towarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addTowary dodajT = new addTowary(towary,settings.vat);
            dodajT.ShowDialog();
            towary = dodajT.towary;
            towary = towary.OrderBy(x => x.indeks).ToList();
            odswiezT();
        }

        //Dodawanie faktury
        private void fakturęToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFaktura dodajF = new addFaktura(dostawcy, towary, faktury, settings.vat);
            dodajF.ShowDialog();
            towary = dodajF.towary;
            towary = towary.OrderBy(x => x.indeks).ToList();
            faktury = dodajF.faktury;
            faktury = faktury.OrderBy(x => x.dostawca).ToList();

            odswiezT();
            odswiezF();
        }

        //Przy wyłączaniu
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wlaczony == 1)
            {
                if (!settings.zapis)
                {
                    DialogResult res = MessageBox.Show("Zapisać zmiany przed wyjściem?", "Zakończ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        wlaczony--;
                        zapisz();
                    }
                    else if (res == DialogResult.No)
                    {
                        wlaczony--;
                        zapiszUst();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    wlaczony--;
                    zapisz();
                }
            }
            else
            {
                wlaczony--;
                zapiszUst();
            }
        }

        //Zapisywanie bazy
        private void zapisz()
        {
            StreamWriter wr;
            XmlSerializer serializer;
            if (towary.Count > 0)
            {
                wr = new StreamWriter(@"baza\towary.xml");
                serializer = new XmlSerializer(typeof(List<towar>));
                serializer.Serialize(wr, towary);
                wr.Flush();
                wr.Close();
            }

            if (faktury.Count > 0)
            {
                wr = new StreamWriter(@"baza\faktury.xml");
                serializer = new XmlSerializer(typeof(List<faktura>));
                serializer.Serialize(wr, faktury);
                wr.Flush();
                wr.Close();
            }

            if(dostawcy.Count > 0)
            {
                wr = new StreamWriter(@"baza\dostawcy.xml");
                serializer = new XmlSerializer(typeof(List<string>));
                serializer.Serialize(wr, dostawcy);
                wr.Flush();
                wr.Close();
            }
            zapiszUst();         
        }

        private void zapiszUst()
        {
            StreamWriter wr;

            wr = new StreamWriter(@"baza\ustawienia.srk");
            wr.WriteLine(settings.vat);
            wr.WriteLine(settings.zapis);
            wr.WriteLine(settings.wczyt);
            wr.WriteLine(settings.firma);
            wr.WriteLine(wlaczony);
            wr.WriteLine(settings.IC_token);
            wr.WriteLine(settings.IC_number);
            wr.Flush();
            wr.Close();
        }

        //Wczytywanie bazy
        private void wczytaj()
        {
            StreamReader r;
            XmlSerializer serializer;

            bool sciezka = File.Exists(@"baza\towary.xml");
            if (sciezka)
            {
                r = new StreamReader(@"baza\towary.xml");
                serializer = new XmlSerializer(typeof(List<towar>));
                towary = (List<towar>)serializer.Deserialize(r);
                r.Close();
                towary = towary.OrderBy(x => x.indeks).ToList();
            }

            sciezka = File.Exists(@"baza\dostawcy.xml");
            if (sciezka)
            {
                r = new StreamReader(@"baza\dostawcy.xml");
                serializer = new XmlSerializer(typeof(List<string>));
                dostawcy = (List<string>)serializer.Deserialize(r);
                r.Close();
                dostawcy = dostawcy.OrderBy(x => x).ToList();
            }
            
            sciezka = File.Exists(@"baza\faktury.xml");
            if (sciezka)
            {
                r = new StreamReader(@"baza\faktury.xml");
                serializer = new XmlSerializer(typeof(List<faktura>));
                faktury = (List<faktura>)serializer.Deserialize(r);
                r.Close();
                faktury = faktury.OrderBy(x => x.dostawca).ToList();
            }
        }

        //Przycisk zapisz
        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zapisz();
        }

        //Przycisk wczytaj
        private void wczytajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wczytaj();
            odswiezF();
            odswiezT();
        }

        //Dodawanie dostawcy
        private void dostawcęToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addDostawca dodajD = new addDostawca(dostawcy);
            dodajD.ShowDialog();
            dostawcy = dodajD.dostawcy;
            dostawcy = dostawcy.OrderBy(x => x).ToList();
        }

        //Edycja towaru
        private void editT()
        {
            int ind = towary.FindIndex(x => x.indeks == listView1.SelectedItems[0].Text);
            edytujTowar edytujT = new edytujTowar(towary, ind, settings.vat);
            edytujT.ShowDialog();

            towary = edytujT.towary;
            odswiezT();
        }

        //Towar doubleclick
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            editT();
        }

        //Towar - enter
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                editT();
            }
        }

        //Sortowanie towarów
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    if(sortT == 1)
                    {
                        towary = towary.OrderByDescending(x => x.indeks).ToList();
                        sortT = -1;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.indeks).ToList();
                        sortT = 1;
                    }
                    break;

                case 1:
                    if(sortT == 2)
                    {
                        towary = towary.OrderByDescending(x => x.nazwa).ToList();
                        sortT = -2;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.nazwa).ToList();
                        sortT = 2;
                    }
                    break;

                case 2:
                    if(sortT == 3)
                    {
                        towary = towary.OrderByDescending(x => x.netto).ToList();
                        sortT = -3;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.netto).ToList();
                        sortT = 3;
                    }
                    break;

                case 3:
                    if(sortT == 4)
                    {
                        towary = towary.OrderByDescending(x => x.brutto).ToList();
                        sortT = -4;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.brutto).ToList();
                        sortT = 4;
                    }
                    break;

                case 4:
                    if(sortT == 5)
                    {
                        towary = towary.OrderByDescending(x => x.stan).ToList();
                        sortT = -5;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.stan).ToList();
                        sortT = 5;
                    }
                    break;

                case 5:
                    if(sortT == 6)
                    {
                        towary = towary.OrderByDescending(x => x.uwagi).ToList();
                        sortT = -6;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.uwagi).ToList();
                        sortT = 6;
                    }
                    break;
                case 6:
                    if (sortT == 7)
                    {
                        towary = towary.OrderByDescending(x => x.ostatni).ToList();
                        sortT = -7;
                    }
                    else
                    {
                        towary = towary.OrderBy(x => x.ostatni).ToList();
                        sortT = 7;
                    }
                    break;

                default:
                    break;
            }
            odswiezT();
        }

        //Sortowanie faktur
        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    if (sortF == 1)
                    {
                        faktury = faktury.OrderByDescending(x => x.dostawca).ToList();
                        sortF = -1;
                    }
                    else
                    {
                        faktury = faktury.OrderBy(x => x.dostawca).ToList();
                        sortF = 1;
                    }
                    break;

                case 1:
                    if (sortF == 2)
                    {
                        faktury = faktury.OrderByDescending(x => x.nr_faktury).ToList();
                        sortF = -2;
                    }
                    else
                    {
                        faktury = faktury.OrderBy(x => x.nr_faktury).ToList();
                        sortF = 2;
                    }
                    break;

                case 2:
                    if (sortF == 3)
                    {
                        faktury = faktury.OrderByDescending(x => x.data).ToList();
                        sortF = -3;
                    }
                    else
                    {
                        faktury = faktury.OrderBy(x => x.data).ToList();
                        sortF = 3;
                    }
                    break;

                case 3:
                    if (sortF == 4)
                    {
                        faktury = faktury.OrderByDescending(x => x.wartosc).ToList();
                        sortF = -4;
                    }
                    else
                    {
                        faktury = faktury.OrderBy(x => x.wartosc).ToList();
                        sortF = 4;
                    }
                    break;


                default:
                    break;
            }
            odswiezF();
        }

        //Pokaż ustawienia
        private void ustawieniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.ShowDialog();
            label1.Text = settings.firma;
            label2.Text = settings.firma;
            if (settings.firma.Length > 0)
                this.Text = "Magazyn - " + settings.firma;
            else
                this.Text = "Magazyn";
        }

        //Koniec programu
        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Edycja faktury
        private void editF()
        {
            int ind = faktury.FindIndex(x => x.dostawca == listView2.SelectedItems[0].Text && x.nr_faktury == listView2.SelectedItems[0].SubItems[1].Text);
            edytujFaktura edytujF = new edytujFaktura(dostawcy, towary, faktury, settings.vat, ind);
            edytujF.ShowDialog();

            towary = edytujF.towary;
            faktury = edytujF.faktury;
            odswiezF();
            odswiezT();
        }

        //Faktura doubleclick
        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            editF();
        }

        //Towar prawy edycja
        private void edytujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editT();
        }

        //Faktura enter
        private void listView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                editF();
            }
        }

        //Towar prawy usuń
        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ind = towary.FindIndex(x => x.indeks == listView1.SelectedItems[0].Text);
            DialogResult res = MessageBox.Show("Na pewno skasować towar?", "Potwierdź", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                towary.RemoveAt(ind);
                odswiezT();
            }
        }

        //prawy przycisk towary
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                contextMenuStrip1.Enabled = false;
            else
                contextMenuStrip1.Enabled = true;
        }

        //prawy przycisk faktury
        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                contextMenuStrip2.Enabled = false;
            else
                contextMenuStrip2.Enabled = true;

        }

        private void usuńToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int ind = faktury.FindIndex(x => x.dostawca == listView2.SelectedItems[0].Text && x.nr_faktury == listView2.SelectedItems[0].SubItems[1].Text);
            DialogResult res = MessageBox.Show("Na pewno skasować fakturę?", "Potwierdź", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                faktury.RemoveAt(ind);
                odswiezF();
            }
        }

        private void pokażFakturyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            towarFaktury pokazF = new towarFaktury(faktury, listView1.SelectedItems[0].Text);
            pokazF.ShowDialog();
        }

        private void edytujToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            editF();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            odswiezT();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            timer2.Stop();
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            odswiezF();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                textBox1.Select();
            else
                textBox2.Select();
        }

        private void drukujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Jeszcze nie ma, ale może kiedyś będzie. \nbtw. w O programie jest easter egg.");
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oProgramie oP = new oProgramie();
            oP.ShowDialog();
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            string znak = "lol";
            KeysConverter kc = new KeysConverter();
            if (tabControl1.SelectedIndex == 0 && !textBox1.Focused)
            {
                if (e.KeyCode == Keys.Back && textBox1.Text.Length > 0)
                {
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                    textBox1.Select();
                    textBox1.SelectionStart = textBox1.Text.Length;
                }
                else if (e.KeyValue >= ((int)Keys.NumPad0) && e.KeyValue <= ((int)Keys.NumPad9))
                {
                    znak = (e.KeyValue - ((int)Keys.NumPad0)).ToString();
                }
                else
                {
                    znak = kc.ConvertToString(e.KeyData);
                }

                if (znak.Length > 1)
                    return;

                textBox1.Text += znak;
                textBox1.Select();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            else if (tabControl1.SelectedIndex == 1 && !textBox2.Focused)
            {
                if (e.KeyCode == Keys.Back && textBox2.Text.Length > 0)
                {
                    textBox2.Text = textBox2.Text.Substring(0, textBox2.Text.Length - 1);
                    textBox2.Select();
                    textBox2.SelectionStart = textBox2.Text.Length;
                }
                else if (e.KeyValue >= ((int)Keys.NumPad0) && e.KeyValue <= ((int)Keys.NumPad9))
                {
                    znak = (e.KeyValue - ((int)Keys.NumPad0)).ToString();
                }
                else
                {
                    znak = kc.ConvertToString(e.KeyData);
                }

                if (znak.Length > 1)
                    return;

                textBox2.Text += znak;
                textBox2.Select();
                textBox2.SelectionStart = textBox2.Text.Length;
            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void excelxlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Dokument Excel (*.xls)|*.xls";
            sfd.FileName = "export.xls";
            sfd.InitialDirectory = Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet1;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet1 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                xlWorkSheet1.Name = "Magazyn";

                int i = 2;

                xlWorkSheet1.Cells[1, 1] = "Indeks";
                xlWorkSheet1.Cells[1, 2] = "Nazwa";
                xlWorkSheet1.Cells[1, 3] = "Stan";
                xlWorkSheet1.Cells[1, 4] = "Cena zakupu";
                foreach (ListViewItem lvi in listView1.Items)
                {
                    xlWorkSheet1.Cells[i, 1].NumberFormat = "@";
                    xlWorkSheet1.Cells[i, 1] = lvi.Text;

                    xlWorkSheet1.Cells[i, 2].NumberFormat = "@";
                    xlWorkSheet1.Cells[i, 2] = lvi.SubItems[1].Text;

                    xlWorkSheet1.Cells[i, 3].NumberFormat = "";
                    xlWorkSheet1.Cells[i, 3] = lvi.SubItems[4].Text;

                    xlWorkSheet1.Cells[i, 4].Style = xlWorkBook.Styles["Currency"];
                    xlWorkSheet1.Cells[i, 4] = double.Parse(lvi.SubItems[6].Text.Remove(lvi.SubItems[6].Text.Length - 3));

                    i++;
                }


                
                xlWorkSheet1.Columns.AutoFit();

                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            odswiezT();
        }

        private void obliczWartośćMagazynuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double suma = 0;
            foreach (towar t in towary)
            {
                suma += t.ostatni * t.stan;
            }
            DialogResult res = MessageBox.Show(suma + " zł\nZapisać do schowka?", "Łączna wartość magazynu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(res == DialogResult.Yes)
            {
                Clipboard.SetText(suma.ToString());
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void synchronizacjaZInterCarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IC_chooseDates dates = new IC_chooseDates();
            dates.ShowDialog();

            if (!dates.sync)
                return;

            int startYear;
            int endYear;

            if (dates.full)
            {
                if (MessageBox.Show("Na pewno uruchomić pełną synchronizację?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
                deleteIC();
                startYear = 2003;
                endYear = DateTime.Now.Year;
            }
            else
            {
                startYear = dates.start;
                endYear = dates.end;
            }

            
            int year = startYear;
            try
            {
                while (year <= endYear)
                {
                    for (int quarter = 0; quarter < 4; quarter++)
                    {
                        var serverUrl = "https://katalog.intercars.com.pl/api/v2/External/GetInvoices?from=" + year.ToString() + (3*quarter+1).ToString("00") + "01" + "&to=" + year.ToString() + (3 * quarter + 3).ToString("00") + DateTime.DaysInMonth(year, (3 * quarter + 3));

                        var client = new System.Net.WebClient();
                        client.Headers["kh_kod"] = settings.IC_number;
                        client.Headers["token"] = settings.IC_token;
                        string response = client.DownloadString(serverUrl);

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(response);
                        XmlNodeList invoices = xmlDoc.GetElementsByTagName("nag");
                        foreach (XmlElement invoice in invoices)
                        {
                            string id = invoice.GetElementsByTagName("id")[0].InnerText;
                            string number = invoice.GetElementsByTagName("numer")[0].InnerText;

                            if (faktury.Exists(x => x.nr_faktury == number))
                            {
                                Console.WriteLine("Invoice " + number + "exists");
                            }
                            else
                            {

                                faktura nowa_faktura = new faktura();
                                nowa_faktura.nr_faktury = number;
                                string data = invoice.GetElementsByTagName("dat_w")[0].InnerText;
                                nowa_faktura.data = new DateTime(int.Parse(data.Substring(0, 4)), int.Parse(data.Substring(4, 2)), int.Parse(data.Substring(6, 2)));
                                nowa_faktura.dostawca = "Inter Cars";

                                nowa_faktura.wartosc = double.Parse(invoice.GetElementsByTagName("war_n")[0].InnerText, CultureInfo.InvariantCulture);
                                nowa_faktura.przedmioty = new List<rzecz>();

                                serverUrl = "https://katalog.intercars.com.pl/api/v2/External/GetInvoice?id=" + id;

                                client = new System.Net.WebClient();
                                client.Headers["kh_kod"] = settings.IC_number;
                                client.Headers["token"] = settings.IC_token;
                                response = client.DownloadString(serverUrl);

                                XmlDocument xmlDoc2 = new XmlDocument();
                                xmlDoc2.LoadXml(response);
                                XmlNodeList wares = xmlDoc2.GetElementsByTagName("poz");

                                foreach (XmlElement item in wares)
                                {
                                    string index = item.GetElementsByTagName("indeks")[0].InnerText;
                                    if (!towary.Exists(x => x.indeks == index))
                                    {
                                        towar nowy_towar = new towar();
                                        nowy_towar.indeks = index;
                                        nowy_towar.nazwa = UTF8(item.GetElementsByTagName("nazwa")[0].InnerText);
                                        nowy_towar.uwagi = UTF8(item.GetElementsByTagName("opis")[0].InnerText);
                                        towary.Add(nowy_towar);
                                        Console.WriteLine("Adding ware " + index);
                                    }
                                    rzecz nowa_rzecz = new rzecz();
                                    nowa_rzecz.indeks = index;
                                    nowa_rzecz.ile = uint.Parse(item.GetElementsByTagName("ilosc")[0].InnerText);
                                    nowa_rzecz.cena = double.Parse(item.GetElementsByTagName("cena")[0].InnerText, CultureInfo.InvariantCulture);
                                    nowa_faktura.przedmioty.Add(nowa_rzecz);
                                }
                                faktury.Add(nowa_faktura);
                                Console.WriteLine("Added invoice " + number);
                            }
                        }
                        towary = towary.OrderBy(x => x.indeks).ToList();
                        odswiezT();
                        faktury = faktury.OrderBy(x => x.dostawca).ToList();
                        odswiezF();
                    }
                    year++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void deleteIC()
        {
            string errors = "";
            List<string> to_delete = new List<string>();
            bool found = false;
            foreach (var invoice in faktury)
            {
                if(invoice.dostawca == "Inter Cars")
                {
                    foreach (var item in invoice.przedmioty)
                    {
                        found = false;
                        foreach (var invoice2 in faktury)
                        {
                            if (invoice2.dostawca != "Inter Cars")
                            {
                                if (found)
                                    break;

                                foreach (var item2 in invoice2.przedmioty)
                                {
                                    if (found)
                                        break;

                                    if (item.indeks == item2.indeks)
                                    {
                                        errors += item.indeks + "\n";
                                        found = true;
                                    }
                                }
                            }
                        }
                        if (!found)
                        {
                            towary.RemoveAll(x => x.indeks == item.indeks);
                        }
                    }
                    to_delete.Add(invoice.nr_faktury);
                }                
            }

            foreach (var item in to_delete)
            {
                faktury.RemoveAll(x => x.nr_faktury == item);
            }
            odswiezF();
            odswiezT();
            if(errors != "")
                MessageBox.Show(errors, "Indeksy od innego dostawcy");
        }

        private string UTF8(string strFrom)
        {
            byte[] bytes = Encoding.Default.GetBytes(strFrom);
            string strTo = Encoding.UTF8.GetString(bytes);

            return strTo;
        }

        private void sprawdźOstatnieCenyZakupówToolStripMenuItem_Click(object sender, EventArgs e)
        {
            faktury = faktury.OrderByDescending(x => x.data).ToList();
            sortF = -3;
            foreach (towar t in towary)
            {
                foreach (faktura f in faktury)
                {
                    foreach (rzecz p in f.przedmioty)
                    {
                        if (t.indeks == p.indeks)
                        {
                            t.ostatni = p.cena;
                        }
                    }
                }
            }

            odswiezT();
        }       
    }

}
