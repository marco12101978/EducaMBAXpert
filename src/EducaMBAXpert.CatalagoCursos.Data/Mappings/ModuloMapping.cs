using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;

namespace EducaMBAXpert.CatalagoCursos.Data.Mappings
{
    public class ModuloMapping : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(m => m.Aulas)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Modulos");
        }
    }
}
