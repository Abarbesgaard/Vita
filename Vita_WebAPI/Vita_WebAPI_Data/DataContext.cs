using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Data;

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
        modelBuilder.Entity<Video>().HasData(
            new Video
            {
                Id = 1,
                Title = "First Video",
                Description = "This is the first video",
                Url = "https://www.youtube.com/watch?v=1"
            },
            new Video
            {
                Id = 2,
                Title = "Second Video",
                Description = "This is the second video",
                Url = "https://www.youtube.com/watch?v=2"
            },
            new Video
            {
                Id = 3,
                Title = "Third Video",
                Description = "This is the third video",
                Url = "https://www.youtube.com/watch?v=3"
            }
        );
    }
}