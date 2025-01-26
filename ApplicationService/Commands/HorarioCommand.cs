using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Commands
{
    public class HorarioCommand
    {
        public Guid IdHorario { get; set; }
        public string DiaSemana { get; set; }

        public string Hora { get; set; }
        public int VagasDisponiveis { get; set; }

        public Guid IdAula { get; set; }

    }
}
