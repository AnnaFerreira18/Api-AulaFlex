using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.QueryModels
{
    public class QueryInscricao
    {
        public Guid IdInscricao { get; set; }
        public Guid IdAula { get; set; }

        public Guid IdHorario { get; set; }
        public Guid IdColaborador { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; }
    }
}
