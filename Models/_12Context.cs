using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Demo_core.Models
{
    public partial class _12Context : DbContext
    {
        public _12Context()
        {
        }

        public _12Context(DbContextOptions<_12Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product1> Products1 { get; set; }
        public virtual DbSet<Table> Tables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=12;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.UserName).HasColumnName("User_Name");

                entity.Property(e => e.UserPass).HasColumnName("User_Pass");
            });

            modelBuilder.Entity<Product1>(entity =>
            {
                entity.ToTable("Products");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserName)
                    .HasMaxLength(10)
                    .HasColumnName("User_Name");

                entity.Property(e => e.UserPass)
                    .HasMaxLength(10)
                    .HasColumnName("User_Pass");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Table");

                entity.Property(e => e.UserName)
                    .HasMaxLength(10)
                    .HasColumnName("user_name")
                    .IsFixedLength(true);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(10)
                    .HasColumnName("user_password")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
