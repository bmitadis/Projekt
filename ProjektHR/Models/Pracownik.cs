using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjektHR.Models
{
    /// <summary>
    /// Klasa reprezentująca dane pracownika
    /// </summary>
    public class Pracownik
    {
        [Key]
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Pesel { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public decimal Stawka { get; set; }
        [ForeignKey("Umowa")]
        public int IdUmowa { get; set; }
        public virtual Umowa Umowa { get; set; }

        public Pracownik() { }
        public Pracownik(string imie, string nazwisko, string pesel, string adres, string telefon, decimal stawka, Umowa umowa)
        {
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Pesel = pesel;
            this.Adres = adres;
            this.Telefon = telefon;
            this.Umowa = umowa;
            this.Stawka = stawka;
        }

        /// <summary>
        /// Metoda dodająca nowego pracownika
        /// </summary>
        /// <param name="imie"></param>
        /// <param name="nazwisko"></param>
        /// <param name="pesel"></param>
        /// <param name="adres"></param>
        /// <param name="telefon"></param>
        /// <param name="umowa"></param>
        /// <param name="dbcontext"></param>
        public static void DodajPracownika(string imie, string nazwisko, string pesel, string adres, string telefon, decimal stawka, Umowa umowa, ApplicationDbContext dbcontext)
        {
            Pracownik p1 = new Pracownik(imie, nazwisko, pesel, adres, telefon, stawka, umowa);
            try
            {
                dbcontext.Pracownicy.Add(p1);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Metoda usuwająca pracownika z bazy danych
        /// </summary>
        /// <param name="pracownik"></param>
        /// <param name="dbcontext"></param>
        public static void UsunPracownika(Pracownik pracownik, ApplicationDbContext dbcontext)
        {
            try
            {
                dbcontext.Pracownicy.Remove(pracownik);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Metoda wyszukująca pracownika w bazie danych
        /// </summary>
        /// <param name="imie"></param>
        /// <param name="nazwisko"></param>
        /// <param name="pesel"></param>
        /// <param name="adres"></param>
        /// <param name="telefon"></param>
        /// <param name="umowa"></param>
        /// <param name="dbcontext"></param>
        /// <returns></returns>
        public static IEnumerable<Pracownik> SzukajPracownika(string imie, string nazwisko, string pesel, string adres, string telefon, Umowa umowa, ApplicationDbContext dbcontext)
        {

            IEnumerable<Pracownik> results = dbcontext.Pracownicy.Local.Where(p => p.Imie.Contains(imie) && p.Nazwisko.Contains(nazwisko) && p.Pesel.Contains(pesel) && p.Telefon.Contains(telefon) && p.Adres.Contains(adres));
            return results;
        }

        /// <summary>
        /// Nadpisanie metody aby dla obiektu Pracownik.ToString() zwracała Imię i Nazwisko
        /// </summary>
        /// <returns> Imię + Nazwisko</returns>
        public override string ToString()
        {
            return Imie + " " + Nazwisko;
        }

        /// <summary>
        /// Metoda sprawdzająca czy podany numer jest poprawnym numerem PESEL
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns>True - PESEL poprawny, False - PESEL błędny</returns>
        public static bool WalidacjaPesel(string pesel)
        {
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            bool result = false;
            if (pesel.Length == 11)
            {
                int controlSum = SumKontrolna(pesel, weights);
                int controlNum = controlSum % 10;
                controlNum = 10 - controlNum;
                if (controlNum == 10)
                {
                    controlNum = 0;
                }
                int lastDigit = int.Parse(pesel[pesel.Length - 1].ToString());
                result = controlNum == lastDigit;
            }
            return result;
        }
        /// <summary>
        /// Metoda obliczająca sumę kontrolną dla numeru PESEL
        /// </summary>
        /// <param name="input"></param>
        /// <param name="weights"></param>
        /// <param name="offset"></param>
        /// <returns>Metoda zwraca sumę kontrolną</returns>
        private static int SumKontrolna(string input, int[] weights, int offset = 0)
        {
            int controlSum = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                controlSum += weights[i + offset] * int.Parse(input[i].ToString());
            }
            return controlSum;
        }
        /// <summary>
        /// Metoda porównująca dwa obiekty typu pracownik
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Pracownik p = (Pracownik)obj;
                return (Imie == p.Imie) && (Nazwisko == p.Nazwisko) && (Pesel == p.Pesel);
            }
        }
    }
}
