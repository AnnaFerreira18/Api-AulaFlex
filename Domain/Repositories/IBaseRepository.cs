using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity Selecionar(object id);
        IEnumerable<TEntity> ListarTodos();
        int TotalDeItens();

        bool Inserir(TEntity entity);
        bool Atualizar(TEntity entity);
        bool Excluir(object id);
    }
}
