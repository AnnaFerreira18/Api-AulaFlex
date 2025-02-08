using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.QueryModels
{
    public class QueryColaborador
    {
        public Guid IdColaborador { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }

        public IEnumerable<QueryInscricao> Inscricoes { get; set; }
    }
}
