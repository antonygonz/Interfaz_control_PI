using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Inter_control
{
    public partial class Form1 : Form
    {
        SerialPort puerto;
        bool estaconectado;
        static Series dist; //Serie a graficar
        public delegate void AñadirDatosDelegate(String str);
        public AñadirDatosDelegate del;
        public Form1()
        {
            InitializeComponent();

            this.del=new AñadirDatosDelegate((string s) => { tbRecibir.AppendText(s); });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if (!estaconectado)
            {
                try
                {
                    estaconectado = true;
                    string puerto_selec = comboBox1.GetItemText(comboBox1.SelectedItem);
                    puerto = new SerialPort(puerto_selec,9600,Parity.None,8,StopBits.One);

                    puerto.Open();
                    button1.Text = "Cerrar conexion";
                    button1.BackColor = Color.Red;
                    button1.ForeColor=Color.White;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error seleccione puerto");
                }
            }
            else
            {
                estaconectado = false;
                puerto.Close();
                button1.Text = "Abrir Conexion";
                button1.BackColor = Color.Blue;
                button1.ForeColor = Color.White;

            }
        }
        static string recibirDato;
        static double datoTemp, convertirNumero = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Conexion Micro";
            chart1.Titles.Add("lectura distancia Sensor");
            dist = chart1.Series.Add("Distancia");

            chart1.Series["Distancia"].ChartType = SeriesChartType.Line;

            string[] puertos = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(puertos);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (puerto.IsOpen)
            {
                recibirDato = puerto.ReadExisting();

                if (recibirDato != "") ;
                {
                    tbRecibir.Text = recibirDato;

                    if (double.TryParse(recibirDato, out convertirNumero))
                    {
                        datoTemp = convertirNumero;
                    }
                    dist.Points.Add(datoTemp);

                    recibirDato = "";
                    puerto.DiscardInBuffer();
                }
            }
        }
    }
}
