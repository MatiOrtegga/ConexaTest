using ConexaTest.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movies");

                entity.HasKey(m => m.Id);

                entity.Property(m => m.Title)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(m => m.Director)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(m => m.Producer)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(m => m.Description)
                      .HasMaxLength(1000);

                entity.Property(m => m.Source)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(m => m.ExternalId)
                      .HasMaxLength(100);

                entity.Property(m => m.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(m => m.UpdatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(r => r.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(u => u.Role)
                      .WithMany()
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

