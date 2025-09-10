using ContatoRegistro.Doninio.Excessoes;
using System.Text.RegularExpressions;

namespace ContatoRegistro.Doninio.ObjetosValor
{
    public sealed record TelefoneObjetoValor
    {
        public string TelefoneValor { get; init; }

        private static readonly Regex _naoDigitos = new(@"[^\d]", RegexOptions.Compiled);

        public TelefoneObjetoValor(string telefone)
        {
            ClausulaGuardaGeralDominio.ContraNuloVazio(telefone, nameof(TelefoneValor));

            var digitos = _naoDigitos.Replace(telefone, "");

            if (digitos.Length < 10 || digitos.Length > 11)
                throw new ExcessaoDominio("O telefone deve conter 10 ou 11 dígitos (BR).");

            TelefoneValor = digitos; 
        }

        public override string ToString() => TelefoneValor;
    }
}
