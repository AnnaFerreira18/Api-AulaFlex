
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class ColaboradorMap : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            builder.HasKey(x => x.IdColaborador);

            builder.Property(x => x.Nome)
                .IsRequired(false) 
                .HasMaxLength(250);

            builder.Property(x => x.Email)
                .IsRequired() 
                .HasMaxLength(250);

            builder.Property(x => x.Senha)
                .IsRequired(false) 
                .HasMaxLength(200);
        }
    }
}
