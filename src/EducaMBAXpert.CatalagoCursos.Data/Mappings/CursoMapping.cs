using EducaMBAXpert.CatalagoCursos.Domain.Entities;
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


            // Mapeando Tags como uma tabela separada (se for guardar em banco)
            builder.OwnsMany<string>("_tags", b =>
            {
                b.WithOwner().HasForeignKey("CursoId");
                b.Property<string>("Tag").HasColumnName("Tag").IsRequired().HasMaxLength(50);
                b.ToTable("CursoTags");
                b.HasKey("CursoId", "Tag");
            });

            builder.ToTable("Cursos");
        }
    }
}
