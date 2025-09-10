using ContatoRegistro.Doninio.Entidades;

namespace ContatoRegistro.Doninio.RepositorioGateway
{
    public interface IContatoRepository
    {
        Task<Contato> AdicionarAsync(Contato contato, CancellationToken ct = default);
        Task<Contato?> ObterPorIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Contato>> ObterTodosAsync(CancellationToken ct = default);
        Task AtualizarAsync(Contato contato, CancellationToken ct = default);
        Task RemoverAsync(int id, CancellationToken ct = default);
        Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);
    }
}
