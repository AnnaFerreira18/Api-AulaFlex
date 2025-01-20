
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class InscricaoMap : IEntityTypeConfiguration<Inscricao>
    {
        public void Configure(EntityTypeBuilder<Inscricao> builder)
        {
            builder.HasKey(x => x.IdInscricao);

            builder.Property(x => x.IdAula)
                .IsRequired();

            builder.HasOne(x => x.Aula) 
                .WithMany() 
                .HasForeignKey(x => x.IdAula) 
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IdHorario)
                .IsRequired();

            builder.HasOne(x => x.Horario) 
                 .WithMany() 
                 .HasForeignKey(x => x.IdHorario) 
                 .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(x => x.IdColaborador)
                .IsRequired();

            builder.HasOne(x => x.Colaborador) 
                .WithMany() 
                .HasForeignKey(x => x.IdColaborador) 
                .OnDelete(DeleteBehavior.Cascade); 


            builder.Property(x => x.DataInicio)
                .IsRequired(false);

            builder.Property(x => x.DataFim)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

