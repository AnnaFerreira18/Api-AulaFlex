using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Commands
{
    public class AulaCommand
    {
        public Guid IdAula { get; set; }        
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }   
    }
}
