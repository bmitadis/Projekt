using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjektHR.Models
{
    /// <summary>
    /// Klasa reprezentująca dane Umowy
    /// </summary>
    public class Umowa
    {
        public int Id { get; set; }
        public string NazwaUmowy { get; set; }
        public decimal UbEmryt { get; set; }
        public decimal UbRent { get; set; }
        public decimal UbChor { get; set; }
        public decimal UbWypadk { get; set; }
        public decimal FP { get; set; }
        public decimal FGSP { get; set; }


        public Umowa() { }
        public Umowa(string nazwa, decimal ubEmryt, decimal ubRent, decimal ubChor, decimal ubWypadk, decimal FP, decimal FGSP)
        {
            this.NazwaUmowy = nazwa;
            this.UbEmryt = ubEmryt;
            this.UbRent = ubRent;
            this.UbChor = ubChor;
            this.UbWypadk = ubWypadk;
            this.FP = FP;
            this.FGSP = FGSP;
        }
        /// <summary>
        /// Metoda dodająca nową umowę do bazy danych
        /// </summary>
        /// <param name="nazwa">Tytul/nazwa umowy</param>
        /// <param name="ubEmryt">wysokosc składek na ubezpieczenie emerytalne</param>
        /// <param name="ubRent">wysokosc składek na ubezpieczenie rentowe</param>
        /// <param name="ubChor">wysokosc składek na ubezpieczenie chorobowe</param>
        /// <param name="ubWypadk">wysokosc składek na ubezpieczenie wypadkowe</param>
        /// <param name="FP">fundusz pracy</param>
        /// <param name="FGSP">Fundusz Gwarantowanych Świadczeń Pracowniczych</param>
        /// <param name="dbcontext"></param>
        public static void DodajUmowe(string nazwa, decimal ubEmryt, decimal ubRent, decimal ubChor, decimal ubWypadk, decimal FP, decimal FGSP, ApplicationDbContext dbcontext)
        {
            Umowa u1 = new Umowa(nazwa, ubEmryt, ubRent, ubChor, ubWypadk, FP, FGSP);
            try
            {
                dbcontext.Umowy.Add(u1);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Metoda usuwająca nową umowę z bazy danych
        /// </summary>
        /// <param name="umowa">obiekt klasy Umowa</param>
        /// <param name="dbcontext"></param>
        public static void UsunUmowe(Umowa umowa, ApplicationDbContext dbcontext)
        {
            try
            {
                dbcontext.Umowy.Remove(umowa);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Nadpisanie metody aby dla obiektu Umowa.ToString() zwracała Nazwę umowy
        /// </summary>
        /// <returns>NazwaUmowy</returns>
        public override string ToString()
        {
            return NazwaUmowy;
        }
    }
}

