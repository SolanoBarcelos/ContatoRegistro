using ContatoRegistro.Aplication.DTOs;
using ContatoRegistro.Aplication.Service.Interfaces;
using ContatoRegistro.Doninio.Entidades;
using ContatoRegistro.Doninio.ObjetosValor;
using ContatoRegistro.Doninio.RepositorioGateway;
using FluentValidation;

namespace ContatoRegistro.Aplication.Service.UseCases
{
    public sealed class ContatoServico : IContatoServico
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IValidator<CriarContatoDTO> _criarContatoValidator;
        private readonly IValidator<AtualizarContatoDTO> _atualizarContatoValidator;

        public ContatoServico(IContatoRepository contatoRepository,
                              IValidator<CriarContatoDTO> criarContatoValidator,
                              IValidator<AtualizarContatoDTO> atualizarContatoValidator)
        {
            _contatoRepository = contatoRepository;
            _criarContatoValidator = criarContatoValidator;
            _atualizarContatoValidator = atualizarContatoValidator;
        }
      
        public async Task<ContatoDTO> CriarAsync(CriarContatoDTO dto, CancellationToken ct = default)
        {
            await _criarContatoValidator.ValidateAndThrowAsync(dto, ct);

            if (await _contatoRepository.ExisteEmailAsync(dto.Email, ct))
            {
                throw new InvalidOperationException("Email já cadastrado.");
            }

            var contato = new Contato(new NomeObjetoValor(dto.Nome), new EmailObjetoValor(dto.Email), new TelefoneObjetoValor(dto.Telefone));

            var contatoCriado = await _contatoRepository.AdicionarAsync(contato, ct);

            return new ContatoDTO
            {
                Id = contatoCriado.Id,
                Nome = contatoCriado.Nome.NomeValor,
                Email = contatoCriado.Email.EmailValor,
                Telefone = contatoCriado.Telefone.TelefoneValor
            };

        }

        public async Task<ContatoDTO?> ObterPorIdAsync(int id, CancellationToken ct = default)
        {
            var contato = await _contatoRepository.ObterPorIdAsync(id, ct);

            if (contato is null) return null;

            return new ContatoDTO
            {
                Id = contato.Id,
                Nome = contato.Nome.NomeValor,
                Email = contato.Email.EmailValor,
                Telefone = contato.Telefone.TelefoneValor

            };
        }

        public async Task<IEnumerable<ContatoDTO>> ObterTodosAsync(CancellationToken ct = default)
        {
            var listaContatos = await _contatoRepository.ObterTodosAsync(ct);
            return listaContatos.Select(contato => new ContatoDTO
            {
                Id = contato.Id,
                Nome = contato.Nome.NomeValor,
                Email = contato.Email.EmailValor,
                Telefone = contato.Telefone.TelefoneValor
            });
        }

        public async Task<bool> AtualizarAsync(int id, AtualizarContatoDTO dto, CancellationToken ct = default)
        {
            var contatoExistente = await _contatoRepository.ObterPorIdAsync(id, ct);

            if (contatoExistente is null) return false;

            contatoExistente.Atualizar(
                new NomeObjetoValor(dto.Nome),
                new EmailObjetoValor(dto.Email),
                new TelefoneObjetoValor(dto.Telefone)
            );

            await _contatoRepository.AtualizarAsync(contatoExistente, ct);
            return true;
        }


        public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
        {
            var contatoExistente = await _contatoRepository.ObterPorIdAsync(id, ct);

            if (contatoExistente is null) return false;

            await _contatoRepository.RemoverAsync(id, ct);

            return true;
        }
    }
}
