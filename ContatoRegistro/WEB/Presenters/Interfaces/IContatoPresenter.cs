using ContatoRegistro.Aplication.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ContatoRegistro.WEB.Presenters.Interfaces
{
    public interface IContatoPresenter
    {
        IActionResult ApresentarCriado(ContatoDTO dto);
        IActionResult ApresentarEncontrado(ContatoDTO dto);
        IActionResult ApresentarLista(IEnumerable<ContatoDTO> lista);
        IActionResult ApresentarAtualizado(bool sucesso);
        IActionResult ApresentarRemovido(bool sucesso);
        IActionResult ApresentarNaoEncontrado();
        IActionResult ApresentarErroValidacao(IEnumerable<string> erros);
        IActionResult ApresentarErroNegocio(string mensagem);
    }
}
