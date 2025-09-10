namespace ContatoRegistro.Aplication.DTOs
{
    public sealed record CriarContatoDTO
    {
        public string Nome { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Telefone { get; init; } = default!;
    }
}
