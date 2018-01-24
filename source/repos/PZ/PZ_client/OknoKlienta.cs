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

namespace PZ_client
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
    }
}
