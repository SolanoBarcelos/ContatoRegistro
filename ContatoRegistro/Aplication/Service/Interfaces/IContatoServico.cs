using ContatoRegistro.Aplication.DTOs;

namespace ContatoRegistro.Aplication.Service.Interfaces
{
    public interface IContatoServico
    {
        Task<ContatoDTO> CriarAsync(CriarContatoDTO dto, CancellationToken ct = default);
        Task<ContatoDTO?> ObterPorIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<ContatoDTO>> ObterTodosAsync(CancellationToken ct = default);
        Task<bool> AtualizarAsync(int id, AtualizarContatoDTO dto, CancellationToken ct = default);
        Task<bool> RemoverAsync(int id, CancellationToken ct = default);
    }
}
