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
        /// <param name="nazwa"></param>
        /// <param name="ubEmryt"></param>
        /// <param name="ubRent"></param>
        /// <param name="ubChor"></param>
        /// <param name="ubWypadk"></param>
        /// <param name="FP"></param>
        /// <param name="FGSP"></param>
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
        /// <param name="umowa"></param>
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

