using EducaMBAXpert.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaMBAXpert.Alunos.Data.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.Ativo)
                   .IsRequired();

            builder.HasMany(u => u.Enderecos)
                   .WithOne(e => e.Aluno)
                   .HasForeignKey(e => e.AlunoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Matriculas)
                    .WithOne(m => m.Aluno) // Matrícula conhece o Aluno
                    .HasForeignKey(m => m.AlunoId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Alunos");
        }
    }
}
