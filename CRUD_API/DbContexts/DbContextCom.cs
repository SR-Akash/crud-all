using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CRUD_API.Models;

#nullable disable

namespace CRUD_API.DbContexts
{
    public partial class DbContextCom : DbContext
    {
        public DbContextCom()
        {
        }

        public DbContextCom(DbContextOptions<DbContextCom> options)
            : base(options)
        {
        }

        public virtual DbSet<Usertest> Usertests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=10.209.99.144;Initial Catalog=SME;User ID=smeapp;Password=sds#dt454sesa0wdnp@1vpo#98;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Usertest>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Usertest");

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LastActiondatetime).HasColumnType("datetime");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Salt).HasMaxLength(250);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
