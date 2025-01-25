using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Transaction
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}
