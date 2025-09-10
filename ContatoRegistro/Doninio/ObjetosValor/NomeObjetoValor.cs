using ContatoRegistro.Doninio.Excessoes;
using System.Drawing;

namespace ContatoRegistro.Doninio.ObjetosValor
{
    public sealed record NomeObjetoValor
    {

        public string NomeValor { get; init; }

        public NomeObjetoValor(string nome)
        {
            ClausulaGuardaGeralDominio.ContraNuloVazio(nome, nameof(nome));

            NomeValor = nome.Trim();

            if (nome.Length < 2)
                throw new ExcessaoDominio("O nome deve conter pelo menos 2 caracteres.");

            NomeValor = nome;

        }

        public override string ToString() => NomeValor;

    }
}
