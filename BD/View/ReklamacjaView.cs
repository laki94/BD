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
using BD.Controller;

namespace BD.View
{
    /// <summary>
    /// Widok odpowiedzialny za wyświetlenie forma do reklamacji wycieczek
    /// </summary>
    public partial class ReklamacjaView : Form
    {
        /// <summary>
        /// Obiekt przechowujący kontroler.
        /// </summary>
        private ReklamacjaController controller;

        /// <summary>
        /// Zmienna przechowująca pesel aktualnie zalogowanego użytkownika
        /// </summary>
        private string _uzytkownik;

        /// <summary>
        /// Obiekt przechowujący klasę odpowiedzialną za sprawdzanie zmian w bazie.
        /// </summary>
        AktualizacjaController aktReklamacji;

        /// <summary>
        /// Główny bezparametrowy konstruktor okna
        /// </summary>
        public ReklamacjaView()
        {
            InitializeComponent();
            controller = new ReklamacjaController(this);

            aktReklamacji = new AktualizacjaController("Reklamacja");
        }

        /// <summary>
        /// Dodaje reklamację dla zdefiniowanego wcześniej użytkownika
        /// </summary>
        /// <param name="uzytkownik">Aktualny użytkownik</param>
        public ReklamacjaView(string uzytkownik)
        {
            InitializeComponent();
            _uzytkownik = uzytkownik;
            controller = new ReklamacjaController(this);

            aktReklamacji = new AktualizacjaController("Reklamacja");
            controller.WypelnijRezerwacje(uzytkownik);
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie wyłączenia okna po wciśnięciu przycisku "Anuluj".
        /// Usuwa utworzone dotąd w ramach swojego działania niezapisane obiekty.
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_anuluj_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie wyłączenia okna poprzez wciśnięcie "X", program wraca do głównego panelu danego użytkownika.
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void Reklamacja_FormClosing(object sender, FormClosingEventArgs e)
        {
            //najpierw sprawdza, czy user kliknał "X" czy po prostu kliknał sobie jakis przycisk wyłączający okno typu "anuluj"
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult czyZakonczyc = MessageBox.Show("Czy na pewno chcesz zamknąć to okno?", "Zamknięcie okna", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (czyZakonczyc == DialogResult.Yes)
                {
                    this.Dispose();
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
        /// Metoda obsługująca zdarzenie kliknięcia przycisku b_zapisz, odpowiada za wywołanie funkcji
        /// dodającej nową reklamację do bazy i obsługe jej komunikatów.
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_zapisz_Click(object sender, EventArgs e)
        {
            int numerRezerwacji = ((KeyValuePair<int, string>)cb_rezerwacje.SelectedItem).Key;
            int zapisz = controller.DodajReklamacje(numerRezerwacji, _uzytkownik);

            switch (zapisz)
            {
                case 1:
                    MessageBox.Show("Prawidłowo dodano reklamację.", "Dodano reklamację.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.b_zapisz.Enabled = false;
                    break;
                case 0:
                    MessageBox.Show("Wprowadzono nieprawidłowy numer rezerwacji. Błedny format.", "Błąd podczas dodawania reklamacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -1:
                    MessageBox.Show("Wystąpił problem podczas dodawania reklamacji. Błąd z zapisem do bazy danych." , "Błąd podczas dodawania reklamacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -2:
                    MessageBox.Show("Wprowadzono inny numer rezerwacji. Sprawdź poprawność tego numeru.", "Błąd podczas dodawania reklamacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }             
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie zmiany zakładki, odpowiada za wywołanie funkcji pobierającej
        /// numery reklamacji i obsługuje jej komunikaty
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void tc_reklamacje_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tc_reklamacje.SelectedIndex == 1)
            {
                if (controller.PobierzReklamacje(_uzytkownik))
                {
                    return;
                }
                else
                {
                    MessageBox.Show("Wystąpił bład podczas pobierania danych.", "Błąd pobierania danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie kliknięcia komórki w listview, odpowiada za wywołanie
        /// funkcji pobierającej informacje o wybranej wycieczce i obsługuje jej komunikaty
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void lv_reklamacje_ItemActivate(object sender, EventArgs e)
        {
            int pobierz = controller.PobierzInformacjeOReklamacji(((ListView)sender).SelectedItems[0].Tag.ToString(), _uzytkownik);

            switch (pobierz)
            {
                case 1:
                    break;
                case 0:
                    MessageBox.Show("Nieprawidłowy format numeru reklamacji. Błędny format.", "Błąd podczas pobierania danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -1:
                    MessageBox.Show("Wystapił problem podczas pobierania danych.", "Błąd podczas pobierania danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Metoda obsługująca kliknięcie przycisku b_sprawdzPoprawnosc, odpowiada za wywołanie funkcji pobierającej
        /// nazwę wycieczki w celu weryfikacji poprawnego numeru rezerwacji i obsługuje jej komunikaty
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void b_sprawdzPoprawnosc_Click(object sender, EventArgs e)
        {
            int numerRezerwacji = ((KeyValuePair<int, string>)cb_rezerwacje.SelectedItem).Key;
            int pobierz = controller.PobierzNazweWycieczki(numerRezerwacji, _uzytkownik);

            switch (pobierz)
            {
                case 1:
                    b_zapisz.Enabled = true;
                    break;
                case -1:
                    MessageBox.Show("Wprowadź prawidłowy numer rezerwacji. Taka rezerwacja nie istnieje.", "Błąd podczas pobierania danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    b_zapisz.Enabled = false;
                    break;
                case 0:
                    MessageBox.Show("Wprowadź prawidłowy numer rezerwacji. Błędny format.", "Błąd podczas pobierania danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    b_zapisz.Enabled = false;
                    break;
                case -2:
                    MessageBox.Show("Wystapił problem podczas pobierania danych.", "Błąd podczas pobierania danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    b_zapisz.Enabled = false;
                    break;
                default:
                    break;
            }          
        }

        /// <summary>
        /// Metoda implementująca wywołanie funkcji sortującej wiersze w kolumnach
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void sortListViewByColumn(object sender, ColumnClickEventArgs e)
        {
            if (((ListView)sender).Sorting == System.Windows.Forms.SortOrder.Ascending)
                ((ListView)sender).Sorting = System.Windows.Forms.SortOrder.Descending;
            else
                ((ListView)sender).Sorting = System.Windows.Forms.SortOrder.Ascending;
            ((ListView)sender).Sort();
            ((ListView)sender).ListViewItemSorter = new ListViewItemComparer(e.Column, ((ListView)sender).Sorting);
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie kliknięcia na nagłówek kolumny, sortuje zawartość listview według danej kolumny
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void lv_reklamacje_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            sortListViewByColumn(sender, e);
        }

        /// <summary>
        /// Metoda zabezpieczająca przed wprowadzeniem znaków innych niż cyfry do tb_numerRezerwacji
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void tb_numerRezerwacji_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Metoda obsługująca kolejne ticki timera, co 5s uruchamia metode sprawdzającą, czy nastąpiła aktualizacja w bazie danych
        /// </summary>
        /// <param name="sender">Rozpoznanie obiektu wywołującego</param>
        /// <param name="e">Zdarzenia systemowe</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (aktReklamacji.czyBylaAktualizacja())
            {
                controller.PobierzReklamacje(_uzytkownik);
            }
            else
            {
                return;
            }
        }
    }
}
