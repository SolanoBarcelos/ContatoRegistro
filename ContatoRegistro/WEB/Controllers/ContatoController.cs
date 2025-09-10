using ContatoRegistro.Aplication.DTOs;
using ContatoRegistro.Aplication.Service.Interfaces;
using ContatoRegistro.WEB.Presenters.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContatoRegistro.WEB.Controllers
{
    [ApiController]
    [Route("api/v1/contatos/")]
    public sealed class ContatoController : ControllerBase
    {
        private readonly IContatoServico _contatoServico;
        private readonly IContatoPresenter _contatoPresenter;

        public ContatoController (IContatoServico contatoServico, IContatoPresenter contatoPresenter)
        {
            _contatoServico = contatoServico;
            _contatoPresenter = contatoPresenter;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId([FromRoute] int id, CancellationToken ct)
        {
            var contato = await _contatoServico.ObterPorIdAsync(id, ct);

                return contato is not null ? _contatoPresenter.ApresentarEncontrado(contato) 
                : _contatoPresenter.ApresentarNaoEncontrado();
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken ct)
        {
            var contatos = await _contatoServico.ObterTodosAsync(ct);
            return _contatoPresenter.ApresentarLista(contatos);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarContatoDTO dto, CancellationToken ct)
        {
            try
            {
                var contatoCriado = await _contatoServico.CriarAsync(dto, ct);
                return _contatoPresenter.ApresentarCriado(contatoCriado);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return _contatoPresenter.ApresentarErroValidacao(ex.Errors.Select(e => e.ErrorMessage));
            }
            catch (InvalidOperationException ex)
            {
                return _contatoPresenter.ApresentarErroNegocio(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] AtualizarContatoDTO dto, CancellationToken ct)
        {
            try
            {
                var sucesso = await _contatoServico.AtualizarAsync(id, dto, ct);
                return _contatoPresenter.ApresentarAtualizado(sucesso);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return _contatoPresenter.ApresentarErroValidacao(ex.Errors.Select(e => e.ErrorMessage));
            }
            catch (InvalidOperationException ex)
            {
                return _contatoPresenter.ApresentarErroNegocio(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remover([FromRoute] int id, CancellationToken ct)
        {
            var sucesso = await _contatoServico.RemoverAsync(id, ct);
            return _contatoPresenter.ApresentarRemovido(sucesso);
        }

    }
}
