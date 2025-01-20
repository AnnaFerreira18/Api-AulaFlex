

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Horario
    {
        public Guid IdHorario { get; set; }
        public string DiaSemana { get; set; }

        public string Hora {  get; set; }
        public int VagasDisponiveis { get; set; }

        public Guid IdAula { get; set; }

        [ForeignKey("IdAula")]
        public virtual Aula Aula { get; set; }
        public IEnumerable<Inscricao> Inscricoes { get; set; }

        public void Inserir(Guid idAula, string diaSemana, string hora, int vagasDisponiveis)
        {
            IdHorario = Guid.NewGuid();
            IdAula = idAula;
            DiaSemana = diaSemana;
            Hora = hora;
            VagasDisponiveis = vagasDisponiveis;
        }
    }
}
