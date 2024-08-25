using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Data;
#pragma warning disable CS1591
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) 
        : base(options)
    {
        Database.EnsureCreated();
    }
    

    public DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Video>(entity =>
            {
                entity.ToTable("Videos");
                entity.HasKey(e => e.Id).HasName("PK_Videos");
                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Title)
                    .HasColumnName("Title");
                
            }
        );
    }
#pragma warning restore CS1591 
}