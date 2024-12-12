using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace petrolOtomasyon
{
    public partial class YoneticiForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Server=BEYZA;Database=petrolOtomasyon;Trusted_Connection=True;");

        public YoneticiForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Veritabanı bağlantısı için bağlantı dizesi
            string connectionString = "Server=localhost; Database=petrolOtomasyon; Integrated Security=True;";

            // İlk SQL sorgusu: Branch bilgilerini almak
            string branchQuery = "SELECT BranchName, Location, City FROM dbo.Branch";

            try
            {
                // SQL bağlantısını oluşturuyoruz ve açıyoruz
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Branch bilgilerini almak için komut oluşturuyoruz
                    SqlCommand branchCommand = new SqlCommand(branchQuery, connection);

                    // Veriyi okuyoruz
                    SqlDataReader reader = branchCommand.ExecuteReader();

                    // ComboBox'ı temizliyoruz
                    comboBoxBranches.Items.Clear();

                    // Veritabanından veri okuma ve ComboBox'a ekleme
                    while (reader.Read())
                    {
                        string branchName = reader["BranchName"].ToString();
                        string location = reader["Location"].ToString();
                        string city = reader["City"].ToString();

                        // ComboBox'a yeni item ekliyoruz
                        comboBoxBranches.Items.Add($"{branchName} - {location}, {city}");
                    }

                    // Reader'ı kapatıyoruz
                    reader.Close();

                    // İkinci SQL sorgusu: Employee bilgilerini almak
                    string employeeQuery = "SELECT Name, Position, BranchID FROM dbo.Employee";

                    // Employee bilgilerini almak için komut oluşturuyoruz
                    SqlCommand employeeCommand = new SqlCommand(employeeQuery, connection);

                    // Veriyi okuyoruz
                    reader = employeeCommand.ExecuteReader();

                    // lstPersonel listesine personelleri ekliyoruz
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string position = reader["Position"].ToString();
                        int branchId = (int)reader["BranchID"]; // BranchID alınıyor

                        // Personeli listeye ekliyoruz
                        lstPersonel.Items.Add($"{name} - {position} (Şube {branchId})");
                    }

                    // Reader'ı kapatıyoruz
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Branch", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void btnPersonelEkle_Click(object sender, EventArgs e)
        {
            string isim = txtIsim.Text;
            string soyisim = txtSoyisim.Text;
            string pozisyon = txtPozisyon.Text;
            string connectionString = "Server=BEYZA;Database=petrolOtomasyonu;Trusted_Connection=True;";

            lstPersonel.Items.Add(isim + " " + soyisim + " - " + pozisyon);
        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var seciliSatir = dataGridView1.SelectedRows[0];
                string talepDetayi = seciliSatir.Cells["TalepDetayi"].Value.ToString();

                // Yakıt talebini veritabanına onaylama işlemi yapılacak

                MessageBox.Show("Talep onaylandı: " + talepDetayi);
            }
            else
            {
                MessageBox.Show("Lütfen onaylamak için bir talep seçin.");
            }
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var seciliSatir = dataGridView1.SelectedRows[0];
                string talepDetayi = seciliSatir.Cells["TalepDetayi"].Value.ToString();

                // Yakıt talebini veritabanına reddetme işlemi yapılacak

                MessageBox.Show("Talep reddedildi: " + talepDetayi);
            }
            else
            {
                MessageBox.Show("Lütfen reddetmek için bir talep seçin.");
            }
        }
    }
}
