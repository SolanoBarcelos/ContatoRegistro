using ContatoRegistro.Doninio.Excessoes;
using System.Text.RegularExpressions;

namespace ContatoRegistro.Doninio.ObjetosValor
{
    public sealed record EmailObjetoValor
    {
        public string EmailValor { get; init; }

        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public EmailObjetoValor(string email)
        {
            ClausulaGuardaGeralDominio.ContraNuloVazio(email, nameof(EmailValor));

            email = email.Trim();

            if (!EmailRegex.IsMatch(email))
                throw new ExcessaoDominio("O email informado não é válido.");

            this.EmailValor = email; // This é uma redundancia para facilitar a leitura do código.

        }

        public override string ToString() => EmailValor;
    }
}
