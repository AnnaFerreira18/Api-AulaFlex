using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Commands
{
    public class EnviarEmailCommand
    {
        public string Email { get; set; }

        public string Corpo { get; set; }
        public string Assunto { get; set; }
    }
}
