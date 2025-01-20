
namespace Domain.Entities
{
    public class Aula
    {
        public Guid IdAula { get;  set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }

        public IEnumerable<Inscricao> Inscricoes { get; set; }

        public void Inserir(string nome, string categoria, string descricao)
        {
            IdAula = Guid.NewGuid();
            Nome = nome;
            Categoria = categoria;
            Descricao = descricao;
        }
    }
}
