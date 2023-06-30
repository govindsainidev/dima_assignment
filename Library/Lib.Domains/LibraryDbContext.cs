using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lib.Domains
{
    public partial class LibraryDbContext : DbContext
    {
        public LibraryDbContext()
        {
        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BooksLoan> BooksLoans { get; set; }
        public virtual DbSet<Gener> Geners { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-80LF158;Database=library;User ID=awaitsoftwares;Password=awaitsoftwares;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AuthorName).HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Genere)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.GenereId)
                    .HasConstraintName("FK_Books_Books");
            });

            modelBuilder.Entity<BooksLoan>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BooksLoans)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BooksSubscribers_Books");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.BooksLoans)
                    .HasForeignKey(d => d.SubscriberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BooksSubscribers_Subscribers");
            });

            modelBuilder.Entity<Gener>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Firstname).HasMaxLength(400);

                entity.Property(e => e.Lastname).HasMaxLength(400);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
