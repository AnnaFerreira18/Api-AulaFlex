using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAula : IBaseRepository<Aula>
    {
        IEnumerable<Aula> ListarTodasAulas();
        bool AssociarHorarioAula(Guid idHorario, Guid idAula);
    }
}
