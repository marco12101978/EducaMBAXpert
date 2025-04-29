using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.CatalagoCursos.Domain.Entities.EducaMBAXpert.CatalagoCursos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaMBAXpert.CatalagoCursos.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasMany(c => c.Modulos)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            builder.OwnsMany<Tag>("_tags", b =>
            {
                b.WithOwner().HasForeignKey("CursoId");

                b.Property(t => t.Valor)
                 .HasColumnName("Tag")
                 .IsRequired()
                 .HasMaxLength(50);

                b.ToTable("CursoTags");
                b.HasKey("CursoId", "Tag");
            });

            builder.ToTable("Cursos");
        }
    }
}
