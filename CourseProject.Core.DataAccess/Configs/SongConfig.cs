using CourseProject.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseProject.Core.DataAccess.Configs
{
    public class SongConfig : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Tittle).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Genre).IsRequired();
            entity.Property(e => e.FilePath).HasMaxLength(256).IsRequired();
        }
    }
}