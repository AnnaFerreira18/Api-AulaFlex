

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Inscricao
    {
        public Guid IdInscricao { get; set; }
        public Guid IdAula { get; set; }

        [ForeignKey("IdAula")]
        public virtual Aula Aula { get; set; }
        public Guid IdHorario { get; set; }
        [ForeignKey("IdHorario")]
        public virtual Horario Horario { get; set; }
        public Guid IdColaborador { get; set; }
        [ForeignKey("IdColaborador")]
        public virtual Colaborador Colaborador { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; }

        public void Inserir(Guid idAula, Guid idcolaborador, Guid idHorario, DateTime dataInicio, DateTime dataFim, string status)
        {
            IdInscricao = Guid.NewGuid();
            IdColaborador = idcolaborador;
            IdAula = idAula;
            IdHorario = idHorario;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Status = status;
        }
    }
}
