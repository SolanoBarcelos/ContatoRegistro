using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using ContatoRegistro.Aplication.DTOs;                 
using ContatoRegistro.Aplication.Service.UseCases;      
using ContatoRegistro.Doninio.Entidades;               
using ContatoRegistro.Doninio.ObjetosValor;            
using ContatoRegistro.Doninio.RepositorioGateway;      
using ContatoRegistro.Doninio.Excessoes;
using ValidationResult = FluentValidation.Results.ValidationResult;
using ValidationException = FluentValidation.ValidationException;
namespace TestContatoRegistro.UseCases;

public sealed class ContatoServicoTests
{
    private readonly Faker _faker = new("pt_BR");


    private static string Digits(string s) => new string((s ?? "").Where(char.IsDigit).ToArray());

    private (ContatoServico sut,
             Mock<IContatoRepository> repo,
             Mock<IValidator<CriarContatoDTO>> criarValidator,
             Mock<IValidator<AtualizarContatoDTO>> atualizarValidator)
        BuildSut_AsValid()
    {
        var repo = new Mock<IContatoRepository>(MockBehavior.Strict);


        repo.Setup(r => r.ExisteEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var criarValidator = new Mock<IValidator<CriarContatoDTO>>();
        criarValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CriarContatoDTO>>(),
                                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        criarValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CriarContatoDTO>(),
                                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var atualizarValidator = new Mock<IValidator<AtualizarContatoDTO>>();
        atualizarValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<AtualizarContatoDTO>>(),
                                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        atualizarValidator
            .Setup(v => v.ValidateAsync(It.IsAny<AtualizarContatoDTO>(),
                                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new ContatoServico(repo.Object, criarValidator.Object, atualizarValidator.Object);
        return (sut, repo, criarValidator, atualizarValidator);
    }

    private static Contato NovoContato(string nome, string email, string tel, int id = 0)
    {
        var e = new Contato(
            new NomeObjetoValor(nome),
            new EmailObjetoValor(email),
            new TelefoneObjetoValor(tel)
        );

        try
        {
            var prop = typeof(Contato).GetProperty("Id");
            if (prop is not null && prop.CanWrite) prop.SetValue(e, id);
        }
        catch { }

        return e;
    }


    [Fact]
    public async Task CriarAsync_deve_criar_e_mapear_quando_valido()
    {
        var (sut, repo, criarVal, _) = BuildSut_AsValid();

        var dto = new CriarContatoDTO
        {
            Nome = _faker.Name.FullName(),
            Email = _faker.Internet.Email(),
            Telefone = _faker.Phone.PhoneNumber("11 9####-####")
        };

        var criado = NovoContato(dto.Nome, dto.Email, dto.Telefone, id: 123);
        repo.Setup(r => r.AdicionarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(criado);

        var resp = await sut.CriarAsync(dto, CancellationToken.None);

        resp.Should().NotBeNull();
        resp.Nome.Should().Be(dto.Nome);
        resp.Email.Should().Be(dto.Email);
        resp.Telefone.Should().Be(Digits(dto.Telefone));

        repo.Verify(r => r.ExisteEmailAsync(dto.Email, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.AdicionarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Once);
        criarVal.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<CriarContatoDTO>>(),
                                             It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CriarAsync_deve_lancar_InvalidOperation_quando_email_duplicado()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var dto = new CriarContatoDTO
        {
            Nome = _faker.Name.FullName(),
            Email = _faker.Internet.Email(),
            Telefone = _faker.Phone.PhoneNumber("11 9####-####")
        };

        repo.Setup(r => r.ExisteEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var act = async () => await sut.CriarAsync(dto, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email já cadastrado.");

        repo.Verify(r => r.ExisteEmailAsync(dto.Email, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.AdicionarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Never);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObterPorIdAsync_deve_retornar_dto_quando_encontrado()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var id = 7;
        var entidade = NovoContato(_faker.Name.FullName(), _faker.Internet.Email(), _faker.Phone.PhoneNumber("11 9####-####"), id);

        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entidade);

        var resp = await sut.ObterPorIdAsync(id, CancellationToken.None);

        resp.Should().NotBeNull();
        resp!.Nome.Should().Be(entidade.Nome.NomeValor);
        resp.Email.Should().Be(entidade.Email.EmailValor);
        resp.Telefone.Should().Be(entidade.Telefone.TelefoneValor);

        repo.Verify(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObterPorIdAsync_deve_retornar_null_quando_inexistente()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        repo.Setup(r => r.ObterPorIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Contato?)null);

        var resp = await sut.ObterPorIdAsync(99, CancellationToken.None);

        resp.Should().BeNull();
        repo.Verify(r => r.ObterPorIdAsync(99, It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObterTodosAsync_deve_mapear_lista()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var e1 = NovoContato(_faker.Name.FullName(), _faker.Internet.Email(), _faker.Phone.PhoneNumber("11 9####-####"), 1);
        var e2 = NovoContato(_faker.Name.FullName(), _faker.Internet.Email(), _faker.Phone.PhoneNumber("11 9####-####"), 2);

        repo.Setup(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contato> { e1, e2 });

        var lista = (await sut.ObterTodosAsync(CancellationToken.None)).ToList();

        lista.Should().HaveCount(2);
        lista.Select(x => x.Nome).Should().Contain(new[] { e1.Nome.NomeValor, e2.Nome.NomeValor });

        repo.Verify(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObterTodosAsync_deve_retornar_vazio_quando_sem_itens()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        repo.Setup(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contato>());

        var lista = (await sut.ObterTodosAsync(CancellationToken.None)).ToList();

        lista.Should().NotBeNull().And.BeEmpty();
        repo.Verify(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AtualizarAsync_deve_atualizar_quando_encontrado()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var id = 10;
        var existente = NovoContato("Nome Antigo", "antigo@email.com", "11 90000-0000", id);

        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existente);

        Contato? capturado = null;
        repo.Setup(r => r.AtualizarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()))
            .Callback<Contato, CancellationToken>((c, _) => capturado = c)
            .Returns(Task.CompletedTask);

        var dto = new AtualizarContatoDTO
        {
            Nome = _faker.Name.FullName(),
            Email = _faker.Internet.Email(),
            Telefone = _faker.Phone.PhoneNumber("11 9####-####")
        };

        var ok = await sut.AtualizarAsync(id, dto, CancellationToken.None);

        ok.Should().BeTrue();
        repo.Verify(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.AtualizarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Once);

        capturado.Should().NotBeNull();
        capturado!.Nome.NomeValor.Should().Be(dto.Nome);
        capturado.Email.EmailValor.Should().Be(dto.Email);
        capturado.Telefone.TelefoneValor.Should().Be(Digits(dto.Telefone));

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AtualizarAsync_deve_retornar_false_quando_nao_encontrado()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var id = 404;
        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Contato?)null);

        var dto = new AtualizarContatoDTO
        {
            Nome = _faker.Name.FullName(),
            Email = _faker.Internet.Email(),
            Telefone = _faker.Phone.PhoneNumber("11 9####-####")
        };

        var ok = await sut.AtualizarAsync(id, dto, CancellationToken.None);

        ok.Should().BeFalse();
        repo.Verify(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.AtualizarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Never);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AtualizarAsync_deve_lancar_ExcessaoDominio_quando_valores_invalidos_no_objeto_de_valor()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var id = 11;
        var existente = NovoContato("Ok", "ok@a.com", "11 90000-0000", id);
        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existente);

        var dto = new AtualizarContatoDTO
        {
            Nome = "",                
            Email = "inválido",        
            Telefone = ""              
        };

        var act = async () => await sut.AtualizarAsync(id, dto, CancellationToken.None);

        await act.Should().ThrowAsync<ExcessaoDominio>();
        repo.Verify(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.AtualizarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Never);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RemoverAsync_deve_remover_quando_encontrado()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var id = 55;
        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(NovoContato(_faker.Name.FullName(), _faker.Internet.Email(), _faker.Phone.PhoneNumber("11 9####-####"), id));

        repo.Setup(r => r.RemoverAsync(id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var ok = await sut.RemoverAsync(id, CancellationToken.None);

        ok.Should().BeTrue();
        repo.Verify(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.RemoverAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RemoverAsync_deve_retornar_false_quando_nao_encontrado()
    {
        var (sut, repo, _, _) = BuildSut_AsValid();

        var id = 56;
        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contato?)null);

        var ok = await sut.RemoverAsync(id, CancellationToken.None);

        ok.Should().BeFalse();
        repo.Verify(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.RemoverAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        repo.VerifyNoOtherCalls();
    }
}
