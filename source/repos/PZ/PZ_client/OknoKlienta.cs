using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PZ_klient
{
    public partial class OknoKlienta : Form
    {
        public OknoKlienta()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string serwer = textBox1.Text;
            int port = Convert.ToInt16(numericUpDown1.Value);

            try
            {
                TcpClient klient = new TcpClient(serwer, port);
                listBox1.Items.Add("Komunikat z serwera " + serwer + ":" + port);
                listBox1.Update();
                NetworkStream ns = klient.GetStream();
                StreamReader reader = new StreamReader(ns); //odczyt ze strumienia
                char[] znaki = new char[100];
                reader.Read(znaki, 0, 100);
                string komunikat = new string(znaki); //zamienia ascii na string
                listBox1.Items.Add("  " + komunikat);
                listBox1.Update();

                ns.Close();
                klient.Close();
            }
            catch
            {
                listBox1.Items.Add("Nie udało się połaczyć z serwerem");
                listBox1.Update();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string serwer = textBox1.Text;
            int port = Convert.ToInt16(numericUpDown1.Value);

            try
            {
                TcpClient klient = new TcpClient(serwer, port);
                listBox1.Items.Add("Odebrano plik z serwera " + serwer + ":" + port);
                listBox1.Update();
                int LiczbaBajtow;
                string nazwaPliku = string.Empty;
                byte[] danePliku = new byte[klient.ReceiveBufferSize];
                NetworkStream ns = klient.GetStream();

                //okno wyboru miejsca zapisu pliku
                SaveFileDialog miejsceZapisu = new SaveFileDialog();
                miejsceZapisu.Filter = "Wszystkie pliki (*.*)|*.*";
                miejsceZapisu.RestoreDirectory = true;
                miejsceZapisu.Title = "Gdzie chcesz zapisać plik?";
                miejsceZapisu.InitialDirectory = @"C:/";

                if (miejsceZapisu.ShowDialog() == DialogResult.OK)
                    nazwaPliku = miejsceZapisu.FileName;

                //obsługa strumienia pliku
                if (nazwaPliku != string.Empty)
                {
                    FileStream Fs = new FileStream(nazwaPliku, FileMode.OpenOrCreate, FileAccess.Write); //utworzenie strumienia pliku
                    while ((LiczbaBajtow = ns.Read(danePliku, 0, danePliku.Length)) > 0)
                    {
                        Fs.Write(danePliku, 0, LiczbaBajtow); //zapis danych ze strumienia do pliku
                    }
                    Fs.Close();
                }

                listBox1.Items.Add("Plik " + nazwaPliku + " został zapisany.);
                listBox1.Update();
                ns.Close();
                klient.Close();
            }
            catch
            {
                listBox1.Items.Add("Nie udało się połaczyć z serwerem");
                listBox1.Update();
            }

        }
    }
}
