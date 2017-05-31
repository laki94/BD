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
        SqlConnection _polaczenie = null;
        SqlCommand _zapytanie = null;
        Polacz_z_baza _polacz = null;
        List<Pojazd_model> _listaPojazdow = new List<Pojazd_model>();
        List<Reklamacja_model> _listaReklamacji = new List<Reklamacja_model>();
        List<Katalog_kontroler_list> _listaWycieczek = new List<Katalog_kontroler_list>();

        /// <summary>
        /// Główny bezparametrowy konstruktor okna, tworzący okno oraz połączenie z bazą danych.
        /// </summary>
        public Kierownik()
        {
            InitializeComponent();
            l_uzytkownik.Text = "Niezidentyfikowany użytkownik";
            _polacz = new Polacz_z_baza();
            _polaczenie = _polacz.PolaczZBaza();
            if (_polaczenie != null)
            {
                l_polaczenie.Text = "Połączony";
                l_polaczenie.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                l_polaczenie.Text = "Rozłączony";
                l_polaczenie.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// Konstruktor okna z parametrem, pozwalający na przekazanie nazwy użytkownika zalogowanego do systemu 
        /// oraz tworzący połączenie z bazą danych.
        /// </summary>
        /// <param name="uzytkownik">Nazwa użytkownika</param>
        public Kierownik(string uzytkownik)
        {
            InitializeComponent();
            l_uzytkownik.Text = uzytkownik;
            _polacz = new Polacz_z_baza();
            _polaczenie = _polacz.PolaczZBaza();
            if (_polaczenie != null)
            {
                l_polaczenie.Text = "Połączony";
                l_polaczenie.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                l_polaczenie.Text = "Rozłączony";
                l_polaczenie.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// a co to je?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kierownik_Load(object sender, EventArgs e)
        {
            this.ZaladujWycieczki();
            // this.reportViewer1.RefreshReport();
        }

        /// <summary>
        /// Zdarzenie otwiera okno dialogowe pozwalające wprowadzić do bazy nowy pojazd.
        /// </summary>
        /// <param name="sender">Rozpoznanie wciśniętego przycisku</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_dodaj_pojazd_Click(object sender, EventArgs e)
        {
            Pojazd pojazd = new Pojazd();
            pojazd.ShowDialog();
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

        private void tc_kierownik_SelectedIndexChanged(object sender, EventArgs e)
        {   
            // Tutaj pobiera liste od nowa, ponieważ ma się przeładować i uaktualnić.
            if(tc_kierownik.SelectedIndex == 0)
            {
                this.ZaladujWycieczki();
            }
            else if (tc_kierownik.SelectedIndex == 1)
            {
                _listaReklamacji = (new Reklamacja_model()).PobierzReklamacje();

                for (int i = 0; i < _listaReklamacji.Count; i++)
                {
                    ListViewItem reklamacja = new ListViewItem(_listaReklamacji[i].Numer.ToString());
                    lv_reklamacje.Items.Add(reklamacja);
                }
            }
            else if (tc_kierownik.SelectedIndex == 2)
            {
                _listaPojazdow = (new Pojazd_model()).PobierzPojazdy();

                for (int i = 0; i < _listaPojazdow.Count; i++)
                {
                    ListViewItem pojazd = new ListViewItem(_listaPojazdow[i].NumerRejestracyjny.ToString());

                    if (_listaPojazdow[i].Dostepnosc == 1)
                    {
                        pojazd.SubItems.Add("Dostępny");
                    }
                    else
                    {
                        pojazd.SubItems.Add("Niedostępny");
                    }

                    pojazd.SubItems.Add(_listaPojazdow[i].Marka.ToString());
                    pojazd.SubItems.Add(_listaPojazdow[i].Pojemnosc.ToString());
                    if(_listaPojazdow[i].Stan == 1)
                    {
                        pojazd.SubItems.Add("Sprawny");
                    }
                    else
                    {
                        pojazd.SubItems.Add("Awaria");
                    }
                    lv_pojazdy.Items.Add(pojazd);
                }
            }
        }
        public void ZaladujWycieczki()
        {
            _listaWycieczek = (new Katalog_kontroler_list()).PobierzListeDlaKierownika();

            for (int i = 0; i < _listaWycieczek.Count; i++)
            {
                ListViewItem wycieczka = new ListViewItem(_listaWycieczek[i].NazwaWycieczki.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].DataWyjazdu.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].DataPrzyjazdu.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].Opis.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].Promocja.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].Cena.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].Kierowca.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].Pilot.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].MiejsceOdjazdu.ToString());
                wycieczka.SubItems.Add(_listaWycieczek[i].MiejsceDocelowe.ToString());

                lv_wycieczki.Items.Add(wycieczka);
            }
        }

        private void b_usun_pojazd_Click(object sender, EventArgs e)
        {
            //Pobranie wybranego numeru rejestracyjnego z listview
            string numerRejestracyjny = lv_pojazdy.SelectedItems[0].SubItems[0].Text;

            if((new Kierownik_model()).UsunPojazd(numerRejestracyjny))
            {
                MessageBox.Show("Pojazd usunięto poprawnie.", "Usunięcie pojazdu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lv_pojazdy.SelectedItems[0].Remove();
            }
            else
            {
                MessageBox.Show("Napotkano problem podczas usuwania pojazdu", "Usunięcie pojazdu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
