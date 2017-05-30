﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BD
{
    public class Uczestnictwo_model
    {
        private int _idUczestnictwa;
        private int _iczbaOsob;
        private int _numerRezerwacji;

        public int IdUczestnictwa
        {
            get
            {
                return this._idUczestnictwa;
            }
            set
            {
                this._idUczestnictwa = value;
            }
        }

        public int LiczbaOsob
        {
            get
            {
                return this._iczbaOsob;
            }
            set
            {
                this._iczbaOsob = value;
            }
        }
        public int NumerRezerwacji
        {
            get
            {
                return this._numerRezerwacji;
            }
            set
            {
                this._numerRezerwacji = value;
            }
        }

        public List<Uczestnictwo_model> PobierzUczestnictwo()
        {
            List<Uczestnictwo_model> _listaUczestnictw= new List<Uczestnictwo_model>();
            Polacz_z_baza _polacz = new Polacz_z_baza();
            SqlConnection _polaczenie = _polacz.PolaczZBaza();
            SqlCommand _zapytanie = _polacz.UtworzZapytanie("SELECT * FROM Uczestnictwo");

            SqlDataReader reader = _zapytanie.ExecuteReader();
            while (reader.Read())
            {
                Uczestnictwo_model uczestnictwo = new Uczestnictwo_model();

                uczestnictwo.IdUczestnictwa = Convert.ToInt32(reader["id_uczestnictwo"]);
                uczestnictwo.LiczbaOsob = Convert.ToInt32(reader["liczba_osob"]);
                uczestnictwo.NumerRezerwacji = Convert.ToInt32(reader["numer_rezerwacji"]);


                _listaUczestnictw.Add(uczestnictwo);
            }
            _polacz.ZakonczPolaczenie();
            return _listaUczestnictw;
        }

        public bool DodajUczestnictwo(Uczestnictwo_model uczestnictwo)
        {
            Polacz_z_baza _polacz = new Polacz_z_baza();
            SqlConnection _polaczenie = _polacz.PolaczZBaza();
            int idUczestnictwa = 0;

            idUczestnictwa = _polacz.PobierzDaneInt(_polacz.UtworzZapytanie("SELECT id_uczestnictwo FROM Uczestnictwo " +
                     "WHERE id_uczestnictwo = " + uczestnictwo.IdUczestnictwa + ""));

            if (uczestnictwo.IdUczestnictwa == idUczestnictwa)
            {
                //tu cos lepszego potem
                MessageBox.Show("Rezerwacja o podanym numerze istnieje. Nie została ponownie dodana do bazy");
                return false;
            }
            else
            {
                SqlCommand _zapytanie = _polacz.UtworzZapytanie("INSERT INTO Uczestnictwo " +
                "VALUES(" + uczestnictwo.IdUczestnictwa + "," + uczestnictwo.LiczbaOsob + "," + uczestnictwo.NumerRezerwacji + ")");
                _zapytanie.ExecuteNonQuery();
                return true;
            }
          
        }
    }
}
