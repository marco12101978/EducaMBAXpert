using EducaMBAXpert.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.Usuarios.Data.Mappings
{
    public class MatriculasMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.UsuarioId)
                .IsRequired();

            builder.Property(m => m.CursoId)
                .IsRequired();

            builder.Property(m => m.DataMatricula)
                .IsRequired();

            builder.Property(m => m.Ativo)
                .IsRequired();

            builder.HasOne(m => m.Usuario)
                            .WithMany(u => u.Matriculas)
                            .HasForeignKey(m => m.UsuarioId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Matriculas");
        }
    }
}
