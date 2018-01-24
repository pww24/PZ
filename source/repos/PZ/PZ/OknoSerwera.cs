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

namespace PZ
{
    public partial class OknoSerwera : Form
    {
        private TcpListener serwer;

        public OknoSerwera()
        {
            InitializeComponent();
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            IPAddress adresIP;

            // sprawdzanie czy adres IP został wpisany poprawnie
            try
            {
                adresIP = IPAddress.Parse(textBox1.Text);
            }
            catch
            {
                textBox1.Text = String.Empty;
                MessageBox.Show("Niewłaściwy format adresu IP", "Błąd adresu IP", MessageBoxButtons.OK);
                return;
            }
            //

            int port = Convert.ToInt16(numericUpDown1.Value); // konwertowanie portu


            // wysyłanie wpisanego komunikatu z danego adresu IP oraz portu
            try
            {
                serwer = new TcpListener(adresIP, port);
                serwer.Start();
                listBox1.Items.Add("komunikat wysyłany...");
                listBox1.Update();
                System.IO.File.AppendAllText("logs.txt", System.DateTime.Now + " Host= " + adresIP + ":" + port + "  " + textBox2.Text + Environment.NewLine); //logowanie komunikatów do pliku

                button1.Enabled = false;
                button2.Enabled = true;

                Socket newsocket = await serwer.AcceptSocketAsync(); //akceptacja klienta na kanale komunikacyjnym
                if (newsocket.Connected)
                {
                    NetworkStream strumien = new NetworkStream(newsocket);
                    byte[] buf = Encoding.ASCII.GetBytes(textBox2.Text); //string na kod ASCII
                    strumien.Write(buf, 0, buf.Length);

                    strumien.Flush(); //wyzwolenie transmisji
                    strumien.Close();
                }
                newsocket.Close(); //zrywamy połączenie z klientem
                listBox1.Items.Add("Komunikat został wysłany.");
                listBox1.Update();
                serwer.Stop();


            }
            catch
            {
                listBox1.Items.Add("Błąd transmisji");
                listBox1.Update();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            serwer.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
            textBox2.Text = string.Empty;
        }
    }
}
