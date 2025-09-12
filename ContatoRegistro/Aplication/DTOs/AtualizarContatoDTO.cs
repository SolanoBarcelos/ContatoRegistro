namespace ContatoRegistro.Aplication.DTOs
{
    public sealed record class AtualizarContatoDTO
    {
        public required string Nome { get; init; }
        public required string Email { get; init; }
        public required string Telefone { get; init; }
    }


}
