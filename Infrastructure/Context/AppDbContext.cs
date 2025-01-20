using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Colaborador> Colaborador { get; set; }

        public DbSet<Aula> Aula { get; set; }
                                                                                                                                                                      
        public DbSet<Inscricao> Inscricao { get; set; }

        public DbSet<Horario> Horario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .HasDbFunction(typeof(AppDbContext).GetMethod(nameof(FormatarData), new[] { typeof(DateTime), typeof(int) }))
                .HasTranslation(args => new SqlFunctionExpression(
                    "convert",
                    args.Prepend(new SqlFragmentExpression("varchar(10)")),
                    true,
                    new[] { false, true, false },
                    typeof(string),
                    null));

            modelBuilder.ApplyConfiguration(new ColaboradorMap());
            modelBuilder.ApplyConfiguration(new AulaMap());
            modelBuilder.ApplyConfiguration(new InscricaoMap());
            modelBuilder.ApplyConfiguration(new HorarioMap());


        }
        public string FormatarData(DateTime data, int style)
          => throw new NotSupportedException();
    }
}
