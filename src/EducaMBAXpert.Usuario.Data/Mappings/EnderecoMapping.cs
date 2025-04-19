using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaMBAXpert.Usuarios.Data.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Domain.Entities.Endereco>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Endereco> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Rua)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Cidade)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Estado)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Cep)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.UsuarioId)
                   .IsRequired();

            builder.ToTable("Enderecos");
        }
    }
}
