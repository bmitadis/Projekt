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
    /// Klasa reprezentująca dane Wypłaty
    /// </summary>
    public class Wyplata
    {
        [Key]
        public int Id { get; set; }
        public string Okres { get; set; }
        public string Umowa { get; set; }
        public decimal Netto { get; set; }
        public decimal UbEmryt { get; set; }
        public decimal UbRent { get; set; }
        public decimal UbChor { get; set; }
        public decimal UbWypadk { get; set; }
        public decimal FP { get; set; }
        public decimal FGSP { get; set; }
        public decimal Brutto { get; set; }

        [ForeignKey("Pracownik")]
        public int IdPracownik { get; set; }
        public virtual Pracownik Pracownik { get; set; }


        public Wyplata() { }
        public Wyplata(string okres, decimal stawka, Pracownik pracownik)
             
        {
            this.Okres = okres;
            this.Netto = stawka* pracownik.Stawka;
            this.UbEmryt = pracownik.Umowa.UbEmryt*Netto/100;
            this.UbRent = pracownik.Umowa.UbRent * Netto / 100; 
            this.UbChor = pracownik.Umowa.UbChor * Netto / 100; 
            this.UbWypadk = pracownik.Umowa.UbWypadk * Netto / 100; 
            this.FP = pracownik.Umowa.FP * Netto / 100; 
            this.FGSP = pracownik.Umowa.FGSP * Netto / 100;
            this.Umowa = pracownik.Umowa.NazwaUmowy;
            this.Pracownik = pracownik;
            this.Brutto = Netto + UbEmryt + UbRent + UbChor + UbWypadk + FP + FGSP;
        }
        /// <summary>
        /// Metoda dodająca nową wypłatę do bazy danych
        /// </summary>
        /// <param name="okres"></param>
        /// <param name="netto"></param>
        /// <param name="pracownik"></param>
        /// <param name="dbcontext"></param>
        public static void DodajWyplate(string okres, decimal netto, Pracownik pracownik, ApplicationDbContext dbcontext)
        {
            Wyplata w1 = new Wyplata(okres, netto, pracownik);
            try
            {
                dbcontext.Wyplaty.Add(w1);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Metoda usuwająca wypłatę z bazy danych
        /// </summary>
        /// <param name="wyplata"></param>
        /// <param name="dbcontext"></param>
        public static void UsunWyplate(Wyplata wyplata, ApplicationDbContext dbcontext)
        {
            try
            {
                dbcontext.Wyplaty.Remove(wyplata);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
