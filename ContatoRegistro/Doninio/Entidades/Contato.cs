using ContatoRegistro.Doninio.Excessoes;
using ContatoRegistro.Doninio.ObjetosValor;

namespace ContatoRegistro.Doninio.Entidades
{
    public sealed class Contato
    {
        public int Id { get; private set; }
        public NomeObjetoValor Nome { get; private set; }
        public EmailObjetoValor Email { get; private set; }
        public TelefoneObjetoValor Telefone { get; private set; }

        private Contato() { }

        public Contato(NomeObjetoValor nome, EmailObjetoValor email, TelefoneObjetoValor telefone)
        {
            Nome = nome ?? throw new ExcessaoDominio("Nome inválido.");
            Email = email ?? throw new ExcessaoDominio("E-mail inválido.");
            Telefone = telefone ?? throw new ExcessaoDominio("Telefone inválido.");
        }

        public void AtribuirId(int id)
        {
            if (id <= 0) throw new ExcessaoDominio("Id inválido.");
            Id = id;
        }

        public void Atualizar(NomeObjetoValor nome, EmailObjetoValor email, TelefoneObjetoValor telefone)
        {
            Nome = nome ?? throw new ExcessaoDominio("Nome inválido.");
            Email = email ?? throw new ExcessaoDominio("E-mail inválido.");
            Telefone = telefone ?? throw new ExcessaoDominio("Telefone inválido.");
        }

    }
    
}
