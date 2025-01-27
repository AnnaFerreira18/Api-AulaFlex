

namespace Domain.Entities
{
    public class Colaborador
    {
        public Guid IdColaborador { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public IEnumerable<Inscricao> Inscricoes { get; set; }

        public void Inserir(string nome, string email, string senha)
        {
            IdColaborador = Guid.NewGuid();

            Nome = nome.Trim();
            Email = email.Trim().ToLower();
            Senha = Criptografia.EncriptarSha1(senha);
        }

    }
}
