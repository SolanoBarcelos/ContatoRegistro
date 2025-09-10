using ContatoRegistro.Aplication.DTOs;
using ContatoRegistro.WEB.Presenters.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContatoRegistro.WEB.Presenters
{
    public sealed class ContatoPresenter : IContatoPresenter
    {
        public IActionResult ApresentarCriado(ContatoDTO dto)
        => new CreatedResult($"/api/contatos/{dto.Id}", dto); 

        public IActionResult ApresentarEncontrado(ContatoDTO dto)
            => new OkObjectResult(dto);

        public IActionResult ApresentarLista(IEnumerable<ContatoDTO> lista)
            => new OkObjectResult(lista);

        public IActionResult ApresentarAtualizado(bool sucesso)
            => sucesso ? new NoContentResult() : new NotFoundResult();

        public IActionResult ApresentarRemovido(bool sucesso)
            => sucesso ? new NoContentResult() : new NotFoundResult();

        public IActionResult ApresentarNaoEncontrado()
            => new NotFoundResult();

        public IActionResult ApresentarErroValidacao(IEnumerable<string> erros)
            => new BadRequestObjectResult(new { Erros = erros });

        public IActionResult ApresentarErroNegocio(string mensagem)
            => new BadRequestObjectResult(new { Mensagem = mensagem });
    }
}
