using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaMBAXpert.CatalagoCursos.Data.Mappings
{
    public class AulaMapping : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Url)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Duracao)
                .IsRequired();

            builder.ToTable("Aulas");
        }
    }
}
