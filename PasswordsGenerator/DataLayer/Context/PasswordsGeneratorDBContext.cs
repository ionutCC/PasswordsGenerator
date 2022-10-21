using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataLayer.Entities;

namespace DataLayer.Context
{
    public partial class PasswordsGeneratorDBContext : DbContext
    {
        public PasswordsGeneratorDBContext()
        {
        }

        public PasswordsGeneratorDBContext(DbContextOptions<PasswordsGeneratorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserPasswordGenerated> UserPasswordGenerated { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Persist Security Info=False;Initial Catalog=PasswordsGeneratorDB;Server=LAPTOP-IT207G8G; Integrated Security=SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPasswordGenerated>(entity =>
            {
                entity.HasKey(e => e.UserCountor)
                    .HasName("PK__UserPass__F48206A6083B1D78");

                entity.Property(e => e.GeneratedPassword).HasMaxLength(8);

                entity.Property(e => e.PasswordGenerationDatetime).HasColumnType("datetime");

                entity.Property(e => e.UserID).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
