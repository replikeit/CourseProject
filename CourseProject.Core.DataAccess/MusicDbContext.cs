using CourseProject.Core.DataAccess.Configs;
using CourseProject.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Core.DataAccess
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SongConfig());
        }
    } 
}