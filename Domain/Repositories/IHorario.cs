using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IHorario : IBaseRepository<Horario>
    {
        IEnumerable<dynamic> ListarHorariosPorAula(Guid aulaId);

        IEnumerable<dynamic> ListarHorariosDisponiveis();
    }
}
