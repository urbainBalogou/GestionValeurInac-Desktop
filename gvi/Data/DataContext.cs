// Data/DataContext.cs
using gvi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace gvi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Commune> Communes { get; set; }
        public DbSet<Fonction> Fonctions { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<TypeValeur> TypeValeurs { get; set; }
        public DbSet<Valeur> Valeurs { get; set; } 
        public DbSet<Entree> Entrees { get; set; }
        public DbSet<EntreeValeur> EntreeValeurs { get; set; }
        public DbSet<Demande> Demandes { get; set; }
        public DbSet<DemandeValeur> DemandeValeurs { get; set; }
        public DbSet<Sortie> Sorties { get; set; }
        public DbSet<SortieValeur> SortieValeurs { get; set; }

        public DbSet<Stockage> Stockages { get; set; }




        //public DbSet<Utilisateur> Utilisateurs { get; set; } // Si tu as une classe Utilisateur

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Remplace "URBAI" et "gvi_database" sont les informations de la base de données et du serveur 
            optionsBuilder.UseSqlServer("Server=URBAIN;Database=gvi_database;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Commune)
                .WithMany(c => c.Employes)
                .HasForeignKey(e => e.CommuneId);

            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Fonction)
                .WithMany(f => f.Employes)
                .HasForeignKey(e => e.FonctionId);

            modelBuilder.Entity<Valeur>()
                .HasOne(v => v.TypeValeur)           // Relation un-à-plusieurs entre Valeur et TypeValeur
                .WithMany(t => t.Valeurs)             // Un TypeValeur peut avoir plusieurs Valeurs
                .HasForeignKey(v => v.typeValeurId);

            modelBuilder.Entity<Stockage>()
            .HasIndex(s => new { s.CommuneId, s.ValeurId })
            .IsUnique();


            modelBuilder.Entity<Sortie>()
                .HasOne(s => s.Demande)
                .WithMany()
                .HasForeignKey(s => s.DemandeId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Sortie>()
                .HasOne(s => s.Employe)
                .WithMany()
                .HasForeignKey(s => s.EmployeId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }

}
