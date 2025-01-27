using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IInscricao : IBaseRepository<Inscricao>
    {
        IEnumerable<dynamic> ListarInscricoesPorColaborador(Guid idColaborador);

        int ObterVagasRestantes(string nomeAula, string diaSemana, string hora);

        IEnumerable<dynamic> TotalInscricoesPorCategoria();

        bool CancelarInscricao(Guid inscricaoId);
        bool VerificarDisponibilidadeHorario(Guid horarioId);

        bool VerificarInscricaoExistente(Guid colaboradorId, Guid aulaId, Guid horarioId);
    }
}
