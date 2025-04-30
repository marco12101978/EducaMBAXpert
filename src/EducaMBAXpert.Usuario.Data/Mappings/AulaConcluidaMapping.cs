using EducaMBAXpert.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaMBAXpert.Usuarios.Data.Mappings
{
    public class AulaConcluidaMapping : IEntityTypeConfiguration<AulaConcluida>
    {
        public void Configure(EntityTypeBuilder<AulaConcluida> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AulaId)
                .IsRequired();

            builder.Property(a => a.DataConclusao)
                .IsRequired();

            builder.HasOne(a => a.Matricula)
                .WithMany(m => m.AulasConcluidas)
                .HasForeignKey(a => a.MatriculaId);

            builder.ToTable("AulasConcluidas");
        }
    }
}
