using ContatoRegistro.Doninio.Excessoes;
using ContatoRegistro.Doninio.ObjetosValor;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContatoRegistro.TestesUnitarios.ObjetosValorDoninio
{
    public sealed class EmailObjetoValorTests
    {
        [Theory]
        [InlineData("user@example.com")]
        [InlineData("USER@EXAMPLE.COM")]
        [InlineData("user.name+alias@empresa.com.br")]
        public void Ctor_ComEmailValido_NaoDeveLancar(string email)
        {
            Action act = () => _ = new EmailObjetoValor(email);
            act.Should().NotThrow();
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalido")]
        [InlineData("sem-arroba.com")]
        [InlineData("nome@dominio")]
        public void Ctor_ComEmailInvalido_DeveLancarExcessaoDominio(string email)
        {
            Action act = () => _ = new EmailObjetoValor(email);
            act.Should().Throw<ExcessaoDominio>();
        }


        [Fact]
        public void Ctor_DeveAplicarTrim()
        {
            var original = " user@example.com ";

            var vo = new EmailObjetoValor(original);

            vo.EmailValor.Should().Be("user@example.com");
        }


        [Fact]
        public void Equals_MesmoEmailMesmaCaixa_DeveSerIgual()
        {
            var a = new EmailObjetoValor("user@example.com");
            var b = new EmailObjetoValor("user@example.com");
            (a == b).Should().BeTrue();
        }
    }
}
