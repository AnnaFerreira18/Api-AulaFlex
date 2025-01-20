

namespace Domain.Entities
{
    public class Colaborador
    {
        public Guid IdColaborador { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }

        public IEnumerable<Inscricao> Inscricoes { get; set; }

        public void Inserir(string nome, string email, string senha)
        {
            IdColaborador = Guid.NewGuid();

            Nome = nome.Trim();
            Email = email.Trim().ToLower();
            Senha = senha.Trim().ToLower();
            //Senha = Criptografia.EncriptarSha1(senha);
        }

        public void ApagarSenhaCandidato()
        {
            Senha = null;
        }

    }
}
