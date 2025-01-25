using Domain.Entities;
using Domain.QueryModels;

namespace Domain.Repositories
{
    public interface IColaborador : IBaseRepository<Colaborador>
    {
        IEnumerable<QueryListarColaboradores> ListarTodosColaboradores();

        Colaborador RealizarLoginComEmail(string email, string senha);

        bool ChecarEmailDuplicado(string email);

        bool AlterarSenha(string email, string senha);

        bool RecuperarSenha(string email, string senhaGerada);

        //bool VerificaRecuperacaoSenha(string email);

        bool VerificaSenha(Guid IdColaborador, string senhaAtual);

        bool AlterarEmail(Guid idColaborador, string novoEmail);

    }
}
