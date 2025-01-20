
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityConfiguration
{
    public class HorarioMap : IEntityTypeConfiguration<Horario>
    {
        public void Configure(EntityTypeBuilder<Horario> builder)
        {
            builder.HasKey(x => x.IdHorario);

            builder.Property(x => x.IdAula)
                .IsRequired();

            builder.HasOne(x => x.Aula)
                .WithMany()
                .HasForeignKey(x => x.IdAula)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.DiaSemana)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Hora)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.VagasDisponiveis)
                .IsRequired();
        }
    }
}
