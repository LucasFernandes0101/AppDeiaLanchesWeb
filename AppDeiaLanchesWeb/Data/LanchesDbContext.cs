using AppDeiaLanchesWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

#nullable disable

namespace AppDeiaLanchesWeb
{
    public partial class LanchesDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public LanchesDbContext()
        {
        }

        public LanchesDbContext(DbContextOptions<LanchesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conta> Contas { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<ProdutosPedido> ProdutosPedidos { get; set; }
        public virtual DbSet<ContasPedido> ContasPedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=db_deialanches;Data Source=DESKTOP-5QP3G8J\\SQLEXPRESS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Conta>(entity =>
            {
                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(e => e.Preco).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ProdutosPedido>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataPedido") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataPedido").CurrentValue = DateTime.Now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataPedido").IsModified = false;
                }
            }
            return base.SaveChanges();
        }
    }
}