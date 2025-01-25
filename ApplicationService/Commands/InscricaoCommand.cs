using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Commands
{
    public class InscricaoCommand
    {
        public Guid IdColaborador { get; set; }
        public Guid IdAula { get; set; }
        public Guid IdHorario { get; set; }
    }
}
