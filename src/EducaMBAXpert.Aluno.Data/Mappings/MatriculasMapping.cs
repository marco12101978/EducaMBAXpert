using EducaMBAXpert.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.Alunos.Data.Mappings
{
    public class MatriculasMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.AlunoId)
                .IsRequired();

            builder.Property(m => m.CursoId)
                .IsRequired();

            builder.Property(m => m.DataMatricula)
                .IsRequired();

            builder.Property(m => m.Ativo)
                .IsRequired();


            builder.HasMany(m => m.AulasConcluidas)
                   .WithOne(a => a.Matricula)
                   .HasForeignKey(a => a.MatriculaId)
                   .OnDelete(DeleteBehavior.Cascade);builder.HasKey(m => m.Id);

            builder.ToTable("Matriculas");
        }
    }
}
