namespace ContatoRegistro.Aplication.DTOs
{
    public sealed record ContatoDTO
    {
        public int Id { get; init; }
        public string Nome { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Telefone { get; init; } = default!;

        //public ContatoDTO(int Id, string Nome, string Email, string Telefone)
        //{
        //    this.Id = Id;
        //    this.Nome = Nome;
        //    this.Email = Email;
        //    this.Telefone = Telefone;
        //}
    }

    


}
