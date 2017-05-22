﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BD
{
    public partial class Kierownik : Form
    {
        /// <summary>
        /// Główny bezparametrowy konstruktor okna
        /// </summary>
        public Kierownik()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Konstruktor okna z parametrem, pozwalający na przekazanie nazwy użytkownika zalogowanego do systemu.
        /// </summary>
        /// <param name="uzytkownik">Nazwa użytkownika</param>
        public Kierownik(string uzytkownik)
        {
            InitializeComponent();
            l_uzytkownik.Text = uzytkownik;
        }

        /// <summary>
        /// a co to je?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kierownik_Load(object sender, EventArgs e)
        {
           // this.reportViewer1.RefreshReport();
        }

        /// <summary>
        /// Zdarzenie otwiera okno dialogowe pozwalające wprowadzić do bazy nowy pojazd.
        /// </summary>
        /// <param name="sender">Rozpoznanie wciśniętego przycisku</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_dodaj_pojazd_Click(object sender, EventArgs e)
        {

            SqlConnection polaczenie = new SqlConnection();
            polaczenie.ConnectionString = "Server=bditake.database.windows.net; Database=baza; User Id=bdsql; password=Chuj123123";
            polaczenie.Open();
            SqlCommand zapytanie = new SqlCommand();
            zapytanie.Connection = polaczenie;
            zapytanie.CommandText = "SELECT opis FROM Opinia WHERE id_opini = 1";

            string wartosc = null;
            SqlDataReader reader = zapytanie.ExecuteReader();
            if (reader.Read())
                wartosc = reader.GetString(0);
            reader.Close();
            polaczenie.Close();
            MessageBox.Show(wartosc,"pobranie danych",MessageBoxButtons.OK);
            // Pojazd pojazd = new Pojazd();
            //pojazd.ShowDialog();
        }

        /// <summary>
        /// Zdarzenie obsługujące wyłączenie aplikacji poprzez wciśnięcie "X", program całkowicie kończy swoją pracę
        /// </summary>
        /// <param name="sender">Rozpoznanie wciśniętego przycisku</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void Kierownik_FormClosing(object sender, FormClosingEventArgs e)
        {
           //najpierw sprawdza, czy user kliknał "X" czy po prostu kliknał sobie jakis przycisk wyłączający okno typu "anuluj"
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult czyZakonczyc = MessageBox.Show("Czy na pewno chcesz wyjść?", "Zakończ", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (czyZakonczyc == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Zdarzenie obsługujące wylogowanie z systemu po wciśnięciu przycisku "Wyjdź", po kliknięciu program przechodzi do panelu logowania.
        /// </summary>
        /// <param name="sender">Rozpoznanie wciśniętego przycisku</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_wyjdz_Click(object sender, EventArgs e)
        {
            //klasyczna obsługa wyjścia z programu po klinięciu "X"/przycisku, messagebox pyta, czy zakończyć, jeśtli tak wyacza okno, jesli nie działa dalej
            DialogResult czyZakonczyc = MessageBox.Show("Czy na pewno chcesz się wylogować?", "Wyloguj", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (czyZakonczyc == DialogResult.Yes)
            {
                this.Hide();
                Panel_pracowniczy panel_pracowniczy = new Panel_pracowniczy();
                panel_pracowniczy.Closed += (s, args) => this.Close();
                panel_pracowniczy.Show();

            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Zdarzenie otwiera okno dialogowe pozwalające wprowadzić do bazy nową wycieczkę.
        /// </summary>
        /// <param name="sender">Rozpoznanie wciśniętego przycisku</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_dodaj_wycieczke_Click(object sender, EventArgs e)
        {
            Wycieczka wycieczka = new Wycieczka();
            wycieczka.ShowDialog();
        }

     }
}
