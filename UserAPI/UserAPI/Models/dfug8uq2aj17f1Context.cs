using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UserAPI.Models
{
    public partial class dfug8uq2aj17f1Context : DbContext
    {
        public dfug8uq2aj17f1Context()
        {
        }

        public dfug8uq2aj17f1Context(DbContextOptions<dfug8uq2aj17f1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<TableUser> TableUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Database=dfug8uq2aj17f1; host=ec2-54-246-101-215.eu-west-1.compute.amazonaws.com; port=5432; Username=qnuavllldruxiq; password=3a39919f5963f461db1eba8957ebb1b00293e12e77aea0ffc864114f6404f8ee;SslMode=Prefer;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TableUser>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("table_user");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasColumnName("adress");

                entity.Property(e => e.Authtoken)
                    .IsRequired()
                    .HasColumnName("authtoken");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Hashedpassword)
                    .IsRequired()
                    .HasColumnName("hashedpassword");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Postnummer).HasColumnName("postnummer");

                entity.Property(e => e.Registered).HasColumnName("registered");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt");

                entity.Property(e => e.Stad)
                    .IsRequired()
                    .HasColumnName("stad");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username");
            });
        }
    }
}
