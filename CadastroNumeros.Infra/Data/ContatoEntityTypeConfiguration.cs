using CadastroNumeros.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroNumeros.Infra.Data
{
    public class ContatoEntityTypeConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.OwnsOne(x => x.Email)
                .Property(x => x.Endereco)
                .HasColumnName("Email")
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
