using EducaMBAXpert.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaMBAXpert.Usuarios.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
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
                   .WithOne(e => e.Usuario)
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Matriculas)
                    .WithOne(m => m.Usuario) // Matrícula conhece o Usuario
                    .HasForeignKey(m => m.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Usuarios");
        }
    }
}
