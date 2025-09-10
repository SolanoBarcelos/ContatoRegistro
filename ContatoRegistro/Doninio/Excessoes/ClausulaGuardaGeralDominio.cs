namespace ContatoRegistro.Doninio.Excessoes
{
    public class ClausulaGuardaGeralDominio
    {
        public static void ContraNuloVazio(string? valor, string nomeParametro)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ExcessaoDominio($"O parâmetro '{nomeParametro}' não pode ser nulo ou vazio.");
            }
        }
    }
}
