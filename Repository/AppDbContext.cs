using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region ENTITIES

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AccessToken> AccessTokens { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Client> Clients { get; set; }


    #endregion ENTITIES

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<User>(builder =>
        {
            builder.OwnsOne(u => u.CompanyInformation, c =>
            {
                c.Property(p => p.CompanyName).HasColumnName("CompanyName");
                c.Property(p => p.CompanyDocument).HasColumnName("CompanyDocument");
                c.Property(p => p.MonthlyRevenue).HasColumnName("MonthlyRevenue");
                c.Property(p => p.CompanyDomain).HasColumnName("CompanyDomain");
                c.Property(p => p.BusinessSegment).HasColumnName("BusinessSegment");
                c.Property(p => p.BusinessDescription).HasColumnName("BusinessDescription");
            });

            builder.OwnsOne(u => u.AddressInformation, a =>
            {
                a.Property(p => p.Address).HasColumnName("Address");
                a.Property(p => p.Number).HasColumnName("Number");
                a.Property(p => p.Complement).HasColumnName("Complement");
                a.Property(p => p.Neighborhood).HasColumnName("Neighborhood");
                a.Property(p => p.Zipcode).HasColumnName("Zipcode");
                a.Property(p => p.State).HasColumnName("State");
                a.Property(p => p.City).HasColumnName("City");
            });
        });

        modelBuilder.Entity<RefreshToken>(builder =>
        {
            builder.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.ExpiresAt);
        });

        modelBuilder.Entity<AccessToken>(builder =>
        {
            builder.HasOne(at => at.User)
                .WithMany()
                .HasForeignKey(at => at.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(at => at.UserId);
            builder.HasIndex(at => at.ExpiresAt);
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId);
        });

        modelBuilder.Entity<Client>(builder =>
        {
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId);
        });

        modelBuilder.Model.SetMaxIdentifierLength(30);

        modelBuilder.Model.ToDebugString();
    }
}
