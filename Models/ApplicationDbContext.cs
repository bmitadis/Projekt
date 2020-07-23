using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektHR.Models
{
    /// <summary>
    /// Klasa zapewniająca połączenie z bazą danych
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefConnPrac")
        {

            Database.Initialize(false);

        }

        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<Umowa> Umowy { get; set; }
        public DbSet<Wyplata> Wyplaty { get; set; }

    }
}